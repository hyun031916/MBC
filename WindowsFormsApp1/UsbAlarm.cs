using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    public class UsbAlarm : IDisposable
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, uint Flags);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool UnregisterDeviceNotification(IntPtr Handle);

        List<IntPtr> _notifications = new List<IntPtr>();

        public const int WM_DEVICECHANGE = 0x0219;
        public const int DBT_DEVTYP_DEVICEINTERFACE = 0x05;
        public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000;

        // 연결된 장치 알아내기
        // http://bboogugu.egloos.com/v/591770
        Guid[] GUID_DEVINTERFACE_LIST =
        {
            // GUID_DEVINTERFACE_USB_DEVICE
            new Guid(0xA5DCBF10, 0x6530, 0x11D2, 0x90, 0x1F, 0x00, 0xC0, 0x4F, 0xB9, 0x51, 0xED),
            // GUID_DEVINTERFACE_COMPORT
            new Guid(0x86e0d1e0, 0x8089, 0x11d0, 0x9c, 0xe4, 0x08, 0x00, 0x3e, 0x30, 0x1f, 0x73),
            // GUID_DEVINTERFACE_MODEM
            new Guid(0x2c7089aa, 0x2e0e, 0x11d1, 0xb1, 0x14, 0x00, 0xc0, 0x4f, 0xc2, 0xaa, 0xe4),
            // GUID_DEVINTERFACE_DISK
            new Guid(0x53f56307, 0xb6bf, 0x11d0, 0x94, 0xf2, 0x00, 0xa0, 0xc9, 0x1e, 0xfb, 0x8b),
            // GUID_DEVINTERFACE_HID, 
            new Guid(0x4D1E55B2, 0xF16F, 0x11CF, 0x88, 0xCB, 0x00, 0x11, 0x11, 0x00, 0x00, 0x30),
            // GUID_NDIS_LAN_CLASS
            new Guid(0xad498944, 0x762f, 0x11d0, 0x8d, 0xcb, 0x00, 0xc0, 0x4f, 0xc3, 0x35, 0x8c),
            // GUID_DEVINTERFACE_SERENUM_BUS_ENUMERATOR
            new Guid(0x4D36E978, 0xE325, 0x11CE, 0xBF, 0xC1, 0x08, 0x00, 0x2B, 0xE1, 0x03, 0x18),
            // GUID_DEVINTERFACE_PARALLEL
            new Guid(0x97F76EF0, 0xF883, 0x11D0, 0xAF, 0x1F, 0x00, 0x00, 0xF8, 0x00, 0x84, 0x5C),
            // GUID_DEVINTERFACE_PARCLASS
            new Guid(0x811FC6A5, 0xF728, 0x11D0, 0xA5, 0x37, 0x00, 0x00, 0xF8, 0x75, 0x3E, 0xD1)
        };

        unsafe public UsbAlarm(IntPtr windowHandle)
        {
            DEV_BROADCAST_DEVICEINTERFACE filter = new DEV_BROADCAST_DEVICEINTERFACE();
            filter.dbcc_size = DEV_BROADCAST_DEVICEINTERFACE.Size;
            filter.dbcc_devicetype = DBT_DEVTYP_DEVICEINTERFACE;

            foreach (Guid guid in GUID_DEVINTERFACE_LIST)
            {
                filter.dbcc_classguid = guid;

                DEV_BROADCAST_DEVICEINTERFACE* ptr = &filter;
                IntPtr ptrStruct = new IntPtr(ptr);
                IntPtr hDevNotify = RegisterDeviceNotification(windowHandle, ptrStruct, DEVICE_NOTIFY_WINDOW_HANDLE);
                if (hDevNotify == IntPtr.Zero)
                {
                    Debug.WriteLine("Failed to register: " + guid);
                }
                else
                {
                    _notifications.Add(hDevNotify);
                }
            }
        }

        public void Dispose()
        {
            foreach (IntPtr handle in _notifications)
            {
                UnregisterDeviceNotification(handle);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct DEV_BROADCAST_DEVICEINTERFACE
    {
        public int dbcc_size;
        public int dbcc_devicetype;
        public int dbcc_reserved;
        public Guid dbcc_classguid;
        public char dbcc_name;
        public static readonly int Size = Marshal.SizeOf(typeof(DEV_BROADCAST_DEVICEINTERFACE));
    }
}