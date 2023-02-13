using System.Runtime.InteropServices;

namespace DisposePattern
{
    internal class Program
    {
        /* UnmanagedLibrary instance is created inside a using block to ensure that it's disposed of properly when we're done using it. 
       We then call the GetFunctionPointer method to get a pointer to a function in the library, and call the CallFunction method to call that function with an argument.
       The GetProcAddress function is imported from kernel32.dll using the DllImport attribute, and is used to get a pointer to a function in the library. 
       The Marshal.GetDelegateForFunctionPointer method is then used to create a delegate that can be called to invoke the function. */

        static void Main(string[] args)
        {
            using (var lib = new UnmanagedLibrary("mylibrary.dll"))
            {
                // Do something with the library, for example:
                var functionPtr = GetFunctionPointer(lib, "my_function");
                var result = CallFunction(functionPtr, 42);
                Console.WriteLine($"Result: {result}");
            }
        }

        static IntPtr GetFunctionPointer(UnmanagedLibrary lib, string functionName)
        {
            // Call GetProcAddress to get the pointer to the function
            var functionPtr = GetProcAddress(lib.libraryHandle, functionName);

            if (functionPtr == IntPtr.Zero)
            {
                throw new Exception($"Failed to get function pointer for {functionName}");
            }
            return functionPtr;
        }

        static int CallFunction(IntPtr functionPtr, int arg)
        {
            // Call the function using Marshal.GetDelegateForFunctionPointer
            var function = (Func<int, int>)Marshal.GetDelegateForFunctionPointer(functionPtr, typeof(Func<int, int>));
            return function(arg);
        }

        // Import the GetProcAddress function from kernel32.dll
        [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
    }


    /* This class represents an unmanaged library that is loaded into memory using the LoadLibrary function from kernel32.dll. 
       The class implements the IDisposable interface, so that clients can properly dispose of the library handle and release the memory used by the library.
       The Dispose method first checks whether the Dispose method is being called by a client or the garbage collector. 
       If it's being called by the client, it will release the library handle and mark the object as disposed. 
       If it's being called by the garbage collector, it will only release the library handle.
       The LoadLibrary and FreeLibrary functions are imported from kernel32.dll using the DllImport attribute. 
       The libraryHandle field stores the handle to the loaded library. */
    public class UnmanagedLibrary : IDisposable
    {
        public IntPtr libraryHandle { get; set; }

        // Constructor
        public UnmanagedLibrary(string libraryPath)
        {
            libraryHandle = LoadLibrary(libraryPath);

            if (libraryHandle == IntPtr.Zero)
            {
                throw new Exception($"Failed to load library {libraryPath}");
            }
        }

        // Destructor
        ~UnmanagedLibrary()
        {
            Dispose(false);
        }

        // IDisposable implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Dispose method that does the actual cleanup
        protected virtual void Dispose(bool disposing)
        {
            if (libraryHandle != IntPtr.Zero)
            {
                FreeLibrary(libraryHandle);
                libraryHandle = IntPtr.Zero;
            }
        }

        // Import the LoadLibrary and FreeLibrary functions from kernel32.dll
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string libraryPath);

        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr hModule);
    }
}