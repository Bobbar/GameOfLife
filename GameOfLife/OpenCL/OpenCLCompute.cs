using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Cloo;
using System.Drawing;


namespace GameOfLife.OpenCL
{
    public class OpenCLCompute : IDisposable
    {

        private ComputeContext _context;
        private ComputeCommandQueue _queue;
        private ComputeDevice _device;
        private ComputeProgram _program;

        private ComputeBuffer<int> _gpuCellsA;
        private ComputeBuffer<int> _gpuCellsB;

        private ComputeKernel _computeNextKernel;

        private const int _threadsPerBlock = 32;
        private int _gpuIndex;
        private int2 _dims;

        public OpenCLCompute(int gpuIndex, int2 dims)
        {
            _gpuIndex = gpuIndex;
            _dims = dims;

            Init();
            InitBuffers(dims);
        }

        private void Init()
        {
            var devices = GetDevices();

            _device = devices[_gpuIndex];

            var platform = _device.Platform;

            _context = new ComputeContext(new[] { _device }, new ComputeContextPropertyList(platform), null, IntPtr.Zero);
            _queue = new ComputeCommandQueue(_context, _device, ComputeCommandQueueFlags.None);

            var kernelPath = $@"{Environment.CurrentDirectory}\OpenCL\OCLKernels.cl";
            string clSource;

            using (StreamReader streamReader = new StreamReader(kernelPath))
            {
                clSource = streamReader.ReadToEnd();
            }

            _program = new ComputeProgram(_context, clSource);

            try
            {
                string options = $@"-cl-std=CL2.0";
                _program.Build(new[] { _device }, options, null, IntPtr.Zero);
            }
            catch (BuildProgramFailureComputeException ex)
            {
                string buildLog = _program.GetBuildLog(_device);
                File.WriteAllText("build_error.txt", buildLog);
                Debug.WriteLine(buildLog);
                throw;
            }

            _computeNextKernel = _program.CreateKernel("ComputeNextState");
        }

        private void InitBuffers(int2 dims)
        {
            int len = dims.X * dims.Y;

            _gpuCellsA = new ComputeBuffer<int>(_context, ComputeMemoryFlags.ReadWrite, len);
            _gpuCellsB = new ComputeBuffer<int>(_context, ComputeMemoryFlags.ReadWrite, len);
        }

        private List<ComputeDevice> GetDevices()
        {
            var devices = new List<ComputeDevice>();

            foreach (var platform in ComputePlatform.Platforms)
            {
                foreach (var device in platform.Devices)
                {
                    devices.Add(device);
                }
            }

            return devices;
        }

        private int PadSize(int len)
        {
            if (len < _threadsPerBlock)
                return _threadsPerBlock;

            int mod = len % _threadsPerBlock;
            int padLen = len - mod + _threadsPerBlock;
            return padLen;
        }

        public void ComputeNextState(ref int[,] cells, int steps, Rule rule)
        {
            int len = _dims.X * _dims.Y;
            int2 padDims = new int2() { X = PadSize(_dims.X), Y = PadSize(_dims.Y) };

            _queue.WriteToBuffer(cells, _gpuCellsA, true, new SysIntX2(0, 0), new SysIntX2(0, 0), new SysIntX2(_dims.X, _dims.Y), null);

            for (int i = 0; i < steps; i++)
            {
                _computeNextKernel.SetMemoryArgument(0, _gpuCellsA);
                _computeNextKernel.SetMemoryArgument(1, _gpuCellsB);
                _computeNextKernel.SetValueArgument(2, _dims);
                _computeNextKernel.SetValueArgument(3, len);
                _computeNextKernel.SetValueArgument(4, rule);
                _queue.Execute(_computeNextKernel, null, new long[] { padDims.X, padDims.Y }, new long[] { _threadsPerBlock, _threadsPerBlock }, null);

                var temp = _gpuCellsA;
                _gpuCellsA = _gpuCellsB;
                _gpuCellsB = temp;
            }

            _queue.Finish();

            ReadBuffer(_gpuCellsA, ref cells, len);
        }

        private void ReadBuffer(ComputeBuffer<int> source, ref int[,] dest, int len)
        {
            GCHandle destinationGCHandle = GCHandle.Alloc(dest, GCHandleType.Pinned);
            IntPtr destinationOffsetPtr = Marshal.UnsafeAddrOfPinnedArrayElement(dest, 0);

            Cloo.Bindings.CL10.EnqueueReadBuffer(
                  _queue.Handle,
                  _gpuCellsA.Handle,
                  true,
                  new IntPtr(0 * 4),
                  new IntPtr(len * 4),
                  destinationOffsetPtr,
                  0,
                  null,
                  null);

            destinationGCHandle.Free();
        }

        public void Dispose()
        {
            _context?.Dispose();
            _queue?.Dispose();
            _program?.Dispose();
            _gpuCellsA?.Dispose();
            _gpuCellsB?.Dispose();
            _computeNextKernel?.Dispose();
        }
    }
}
