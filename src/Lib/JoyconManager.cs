using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


using System;

namespace Joycon4CS
{
	public class JoyconManager
	{

		// Settings accessible via Unity
		public bool EnableIMU = true;
		public bool EnableLocalize = true;

		// Different operating systems either do or don't like the trailing zero
		private const ushort vendor_id = 0x57e;
		private const ushort vendor_id_ = 0x057e;
		private const ushort product_l = 0x2006;
		private const ushort product_r = 0x2007;

		public List<Joycon> j = new List<Joycon>(); // Array of all connected Joy-Cons
		static JoyconManager instance;

		public static JoyconManager Instance
		{
			get { return instance; }
		}

		internal void Scan()
		{
			if (instance != null) ;
			instance = this;
			int i = 0;


			bool isLeft = false;
			bool isvalid = true;
			HIDapi.hid_init();

			IntPtr ptr = HIDapi.hid_enumerate(vendor_id, 0x0);
			IntPtr top_ptr = ptr;

			if (ptr == IntPtr.Zero)
			{
				ptr = HIDapi.hid_enumerate(vendor_id_, 0x0);
				if (ptr == IntPtr.Zero)
				{
					HIDapi.hid_free_enumeration(ptr);
					Console.WriteLine("No Joy-Cons found!");
				}
			}
			hid_device_info enumerate;
			while (ptr != IntPtr.Zero) {
				enumerate = (hid_device_info)Marshal.PtrToStructure(ptr, typeof(hid_device_info));
				isvalid = false;
				Console.WriteLine(enumerate.product_id);
				if (enumerate.product_id == product_l || enumerate.product_id == product_r) {
					if (enumerate.product_id == product_l) {
						isvalid = true;
						isLeft = true;
						Console.WriteLine("Left Joy-Con connected.");
					} else if (enumerate.product_id == product_r) {
						isvalid = true;
						isLeft = false;
						Console.WriteLine("Right Joy-Con connected.");
					} else {
						Console.WriteLine("Non Joy-Con input device skipped.");
					}
					if (isvalid)
					{
						IntPtr handle = HIDapi.hid_open_path(enumerate.path);
						HIDapi.hid_set_nonblocking(handle, 1);
						j.Add(new Joycon(handle, EnableIMU, EnableLocalize & EnableIMU, 0.04f, isLeft));
					}
					++i;
				}
				ptr = enumerate.next;
			}
			HIDapi.hid_free_enumeration(top_ptr);
		}

		internal void Start()
		{
			for (int i = 0; i < j.Count; ++i)
			{
				Console.WriteLine(i);
				Joycon jc = j[i];
				byte LEDs = 0x0;
				LEDs |= (byte)(0x1 << i);
				jc.Attach(leds_: LEDs);
				jc.Begin();
			}
		}

		internal void Update()
		{
			for (int i = 0; i < j.Count; ++i)
			{
				Joycon jc = j[i];
				jc.Update();
			}
		}

		internal void OnApplicationQuit()
		{
			for (int i = 0; i < j.Count; ++i)
			{
				Joycon jc = j[i];
				jc.Detach();
			}
		}
	}
}