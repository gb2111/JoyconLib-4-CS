using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
MIT License
Copyright ÂŠ 2006 The Mono.Xna Team

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace Joycon4CS
{

#if WINRT
    [DataContract]
#else
	[Serializable]
#endif
    public struct Quaternion : IEquatable<Quaternion>
    {
#if WINRT
        [DataMember]
#endif
        public double X;
#if WINRT
        [DataMember]
#endif
        public double Y;
#if WINRT
        [DataMember]
#endif
        public double Z;
#if WINRT
        [DataMember]
#endif
        public double W;

		const double radToDeg = (double)(180.0 / Math.PI);
		const double degToRad = (double)(Math.PI / 180.0);


		static Quaternion identity = new Quaternion(0, 0, 0, 1);


        public Quaternion(double x, double y, double z, double w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }


        public Quaternion(Vector3 vectorPart, double scalarPart)
        {
            this.X = vectorPart.X;
            this.Y = vectorPart.Y;
            this.Z = vectorPart.Z;
            this.W = scalarPart;
        }

        public static Quaternion Identity
        {
            get { return identity; }
        }

        public static float PI = (float)Math.PI;

		private static Quaternion ToQ(Vector3 euler)
		{
			var yaw = euler.Y;
			var pitch = euler.X;
			var roll = euler.Z;
			//yaw *= degToRad;
			//pitch *= degToRad;
			//roll *= degToRad;
			double rollOver2 = roll * 0.5f;
			double sinRollOver2 = (double)Math.Sin((double)rollOver2);
			double cosRollOver2 = (double)Math.Cos((double)rollOver2);
			double pitchOver2 = pitch * 0.5f;
			double sinPitchOver2 = (double)Math.Sin((double)pitchOver2);
			double cosPitchOver2 = (double)Math.Cos((double)pitchOver2);
			double yawOver2 = yaw * 0.5f;
			double sinYawOver2 = (double)Math.Sin((double)yawOver2);
			double cosYawOver2 = (double)Math.Cos((double)yawOver2);
			Quaternion result = new Quaternion();
			result.W = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
			result.X = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
			result.Y = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
			result.Z = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;

			return result;
		}


		private static Vector3 FromQ(Quaternion q2)
		{
			Quaternion q = new Quaternion(q2.X, q2.Y, q2.Z, q2.W);
			Vector3 pitchYawRoll = new Vector3(0f, 0f, 0f);
			pitchYawRoll.Y = (float)Math.Atan2(2f * q.X * q.W + 2f * q.Y * q.Z, 1 - 2f * (q.Z * q.Z + q.W * q.W));     // Yaw
			pitchYawRoll.X = (float)Math.Asin(2f * (q.X * q.Z - q.W * q.Y));                             // Pitch
			pitchYawRoll.Z = (float)Math.Atan2(2f * q.X * q.Y + 2f * q.Z * q.W, 1 - 2f * (q.Y * q.Y + q.Z * q.Z));      // Roll
			return new Vector3(
				pitchYawRoll.X //* radToDeg
				,pitchYawRoll.Y //* radToDeg
				,pitchYawRoll.Z //* radToDeg
				);
		}

		public Vector3 eulerAngles
		{
			get
			{
				return Quaternion.FromQ(this);// * radToDeg;
			}
			set
			{
				this = Quaternion.ToQ(value);// * radToDeg);
			}
		}


		public static Quaternion Add(Quaternion quaternion1, Quaternion quaternion2)
        {
            //Syderis
            Quaternion quaternion;
            quaternion.X = quaternion1.X + quaternion2.X;
            quaternion.Y = quaternion1.Y + quaternion2.Y;
            quaternion.Z = quaternion1.Z + quaternion2.Z;
            quaternion.W = quaternion1.W + quaternion2.W;
            return quaternion;
        }


        public static void Add(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
        {
            //Syderis
            result.X = quaternion1.X + quaternion2.X;
            result.Y = quaternion1.Y + quaternion2.Y;
            result.Z = quaternion1.Z + quaternion2.Z;
            result.W = quaternion1.W + quaternion2.W;
        }

        //Funcion aĂąadida Syderis
        public static Quaternion Concatenate(Quaternion value1, Quaternion value2)
        {
            Quaternion quaternion;
            double x = value2.X;
            double y = value2.Y;
            double z = value2.Z;
            double w = value2.W;
            double num4 = value1.X;
            double num3 = value1.Y;
            double num2 = value1.Z;
            double num = value1.W;
            double num12 = (y * num2) - (z * num3);
            double num11 = (z * num4) - (x * num2);
            double num10 = (x * num3) - (y * num4);
            double num9 = ((x * num4) + (y * num3)) + (z * num2);
            quaternion.X = ((x * num) + (num4 * w)) + num12;
            quaternion.Y = ((y * num) + (num3 * w)) + num11;
            quaternion.Z = ((z * num) + (num2 * w)) + num10;
            quaternion.W = (w * num) - num9;
            return quaternion;

        }

        //AĂąadida por Syderis
        public static void Concatenate(ref Quaternion value1, ref Quaternion value2, out Quaternion result)
        {
            double x = value2.X;
            double y = value2.Y;
            double z = value2.Z;
            double w = value2.W;
            double num4 = value1.X;
            double num3 = value1.Y;
            double num2 = value1.Z;
            double num = value1.W;
            double num12 = (y * num2) - (z * num3);
            double num11 = (z * num4) - (x * num2);
            double num10 = (x * num3) - (y * num4);
            double num9 = ((x * num4) + (y * num3)) + (z * num2);
            result.X = ((x * num) + (num4 * w)) + num12;
            result.Y = ((y * num) + (num3 * w)) + num11;
            result.Z = ((z * num) + (num2 * w)) + num10;
            result.W = (w * num) - num9;
        }

        //AĂąadida por Syderis
        public void Conjugate()
        {
            this.X = -this.X;
            this.Y = -this.Y;
            this.Z = -this.Z;
        }

        //AĂąadida por Syderis
        public static Quaternion Conjugate(Quaternion value)
        {
            Quaternion quaternion;
            quaternion.X = -value.X;
            quaternion.Y = -value.Y;
            quaternion.Z = -value.Z;
            quaternion.W = value.W;
            return quaternion;
        }

        //AĂąadida por Syderis
        public static void Conjugate(ref Quaternion value, out Quaternion result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
            result.W = value.W;
        }

        internal static double smo = 0.2;

        public void AddSmo(Quaternion q)
        {
            if(!(double.IsInfinity(q.W) || double.IsInfinity(q.X) || double.IsInfinity(q.Y) || double.IsInfinity(q.Z) ||
                double.IsNaN(q.W) || double.IsNaN(q.X) || double.IsNaN(q.Y) || double.IsNaN(q.Z)
                ))
            {
                if(double.IsInfinity(this.W) || double.IsInfinity(this.X) || double.IsInfinity(this.Y) || double.IsInfinity(this.Z) ||
                    double.IsNaN(this.W) || double.IsNaN(this.X) || double.IsNaN(this.Y) || double.IsNaN(this.Z))
                {
                    this.W = q.W;
                    this.X = q.X;
                    this.Y = q.Y;
                    this.Z = q.Z;
                }
                else { 
                    this.W = smo * q.W + (1.0 - smo) * this.W;
                    this.X = smo * q.X + (1.0 - smo) * this.X;
                    this.Y = smo * q.Y + (1.0 - smo) * this.Y;
                    this.Z = smo * q.Z + (1.0 - smo) * this.Z;
                }
            }
        }

        public static Vector3 AxisToEuler(Vector3 vec, double angle)
        {
            Vector3 pitchYawRoll = new Vector3();

            pitchYawRoll.X = Math.Atan2(vec.Y * Math.Sin(angle) - vec.X * vec.Z * (1 - Math.Cos(angle)), 1 - (vec.Y* vec.Y + vec.Z* vec.Z) * (1 - Math.Cos(angle)));
            pitchYawRoll.Y = Math.Asin(vec.X * vec.Y * (1 - Math.Cos(angle)) + vec.Z * Math.Sin(angle));
            pitchYawRoll.Z = Math.Atan2(vec.X * Math.Sin(angle) - vec.Y * vec.Z * (1 - Math.Cos(angle)), 1 - (vec.X* vec.X + vec.Z* vec.Z) * (1 - Math.Cos(angle)));


            return pitchYawRoll;
            }


        public static Vector3 ToEulerAngles8( Quaternion q)
        {
            // Store the Euler angles in radians
            Vector3 pitchYawRoll = new Vector3();

            double sqw = q.W * q.W;
            double sqx = q.X * q.X;
            double sqy = q.Y * q.Y;
            double sqz = q.Z * q.Z;

            // If quaternion is normalised the unit is one, otherwise it is the correction factor
            double unit = sqx + sqy + sqz + sqw;
            double test = q.X * q.Y + q.Z * q.W;

            if (test > 0.4999f * unit)                              // 0.4999f OR 0.5f - EPSILON
            {
                // Singularity at north pole
                pitchYawRoll.Y = 2f * (float)Math.Atan2(q.X, q.W);  // Yaw
                pitchYawRoll.X = PI * 0.5f;                         // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }
            else if (test < -0.4999f * unit)                        // -0.4999f OR -0.5f + EPSILON
            {
                // Singularity at south pole
                pitchYawRoll.Y = -2f * (float)Math.Atan2(q.X, q.W); // Yaw
                pitchYawRoll.X = -PI * 0.5f;                        // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }
            else
            {
                pitchYawRoll.Y = (float)Math.Atan2(2f * q.Y * q.W - 2f * q.X * q.Z, sqx - sqy - sqz + sqw);       // Yaw
                pitchYawRoll.X = (float)Math.Asin(2f * test / unit);                                             // Pitch
                pitchYawRoll.Z = (float)Math.Atan2(2f * q.X * q.W - 2f * q.Y * q.Z, -sqx + sqy - sqz + sqw);      // Roll
            }

            return pitchYawRoll;
        }

        public static Quaternion CreateFromYawPitchRoll8(double yaw, double pitch, double roll)
        {
            double rollOver2 = roll * 0.5f;
            double sinRollOver2 = (double)Math.Sin((double)rollOver2);
            double cosRollOver2 = (double)Math.Cos((double)rollOver2);
            double pitchOver2 = pitch * 0.5f;
            double sinPitchOver2 = (double)Math.Sin((double)pitchOver2);
            double cosPitchOver2 = (double)Math.Cos((double)pitchOver2);
            double yawOver2 = yaw * 0.5f;
            double sinYawOver2 = (double)Math.Sin((double)yawOver2);
            double cosYawOver2 = (double)Math.Cos((double)yawOver2);
            Quaternion result;
            result.X = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
            result.Y = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;
            result.Z = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
            result.W = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
            return result;
        }

        public static Quaternion EulerToQuaternion(double attitude_radians, double heading_radians, double bank_radians)
        {

            // Assuming the angles are in radians.
            //(x=pitch, y=yaw, z=roll)
             double c1 = Math.Cos(attitude_radians / 2.0);
             double s1 = Math.Sin(attitude_radians / 2.0);
             double c2 = Math.Cos(heading_radians / 2.0);
             double s2 = Math.Sin(heading_radians / 2.0);
             double c3 = Math.Cos(bank_radians / 2.0);
             double s3 = Math.Sin(bank_radians / 2.0);
            Quaternion q = new Quaternion(
                (c1 * c2 * c3 + s1 * s2 * s3),  // w = cos(theta/2)
                (s1 * c2 * c3 - c1 * s2 * s3),  // x = v.i*sin(theta/2)
                (c1 * s2 * c3 + s1 * c2 * s3),  // y = v.j*sin(theta/2)
                (c1 * c2 * s3 - s1 * s2 * c3)); // z = v.k*sin(theta/2)

            return q;
        }

        public static Quaternion CreateFromAxisAngle(Vector3 axis, double angle)
        {

            Quaternion quaternion;
            double num2 = angle * 0.5f;
            double num = (double)Math.Sin((double)num2);
            double num3 = (double)Math.Cos((double)num2);
            quaternion.X = axis.X * num;
            quaternion.Y = axis.Y * num;
            quaternion.Z = axis.Z * num;
            quaternion.W = num3;
            return quaternion;

        }


        public static void CreateFromAxisAngle(ref Vector3 axis, double angle, out Quaternion result)
        {
            double num2 = angle * 0.5f;
            double num = (double)Math.Sin((double)num2);
            double num3 = (double)Math.Cos((double)num2);
            result.X = axis.X * num;
            result.Y = axis.Y * num;
            result.Z = axis.Z * num;
            result.W = num3;

        }


        public static Quaternion CreateFromRotationMatrix(Matrix matrix)
        {
            double num8 = (matrix.M11 + matrix.M22) + matrix.M33;
            Quaternion quaternion = new Quaternion();
            if (num8 > 0f)
            {
                double num = (double)Math.Sqrt((double)(num8 + 1f));
                quaternion.W = num * 0.5f;
                num = 0.5f / num;
                quaternion.X = (matrix.M23 - matrix.M32) * num;
                quaternion.Y = (matrix.M31 - matrix.M13) * num;
                quaternion.Z = (matrix.M12 - matrix.M21) * num;
                return quaternion;
            }
            if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
            {
                double num7 = (double)Math.Sqrt((double)(((1f + matrix.M11) - matrix.M22) - matrix.M33));
                double num4 = 0.5f / num7;
                quaternion.X = 0.5f * num7;
                quaternion.Y = (matrix.M12 + matrix.M21) * num4;
                quaternion.Z = (matrix.M13 + matrix.M31) * num4;
                quaternion.W = (matrix.M23 - matrix.M32) * num4;
                return quaternion;
            }
            if (matrix.M22 > matrix.M33)
            {
                double num6 = (double)Math.Sqrt((double)(((1f + matrix.M22) - matrix.M11) - matrix.M33));
                double num3 = 0.5f / num6;
                quaternion.X = (matrix.M21 + matrix.M12) * num3;
                quaternion.Y = 0.5f * num6;
                quaternion.Z = (matrix.M32 + matrix.M23) * num3;
                quaternion.W = (matrix.M31 - matrix.M13) * num3;
                return quaternion;
            }
            double num5 = (double)Math.Sqrt((double)(((1f + matrix.M33) - matrix.M11) - matrix.M22));
            double num2 = 0.5f / num5;
            quaternion.X = (matrix.M31 + matrix.M13) * num2;
            quaternion.Y = (matrix.M32 + matrix.M23) * num2;
            quaternion.Z = 0.5f * num5;
            quaternion.W = (matrix.M12 - matrix.M21) * num2;

            return quaternion;

        }


        public static void CreateFromRotationMatrix(ref Matrix matrix, out Quaternion result)
        {
            double num8 = (matrix.M11 + matrix.M22) + matrix.M33;
            if (num8 > 0f)
            {
                double num = (double)Math.Sqrt((double)(num8 + 1f));
                result.W = num * 0.5f;
                num = 0.5f / num;
                result.X = (matrix.M23 - matrix.M32) * num;
                result.Y = (matrix.M31 - matrix.M13) * num;
                result.Z = (matrix.M12 - matrix.M21) * num;
            }
            else if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
            {
                double num7 = (double)Math.Sqrt((double)(((1f + matrix.M11) - matrix.M22) - matrix.M33));
                double num4 = 0.5f / num7;
                result.X = 0.5f * num7;
                result.Y = (matrix.M12 + matrix.M21) * num4;
                result.Z = (matrix.M13 + matrix.M31) * num4;
                result.W = (matrix.M23 - matrix.M32) * num4;
            }
            else if (matrix.M22 > matrix.M33)
            {
                double num6 = (double)Math.Sqrt((double)(((1f + matrix.M22) - matrix.M11) - matrix.M33));
                double num3 = 0.5f / num6;
                result.X = (matrix.M21 + matrix.M12) * num3;
                result.Y = 0.5f * num6;
                result.Z = (matrix.M32 + matrix.M23) * num3;
                result.W = (matrix.M31 - matrix.M13) * num3;
            }
            else
            {
                double num5 = (double)Math.Sqrt((double)(((1f + matrix.M33) - matrix.M11) - matrix.M22));
                double num2 = 0.5f / num5;
                result.X = (matrix.M31 + matrix.M13) * num2;
                result.Y = (matrix.M32 + matrix.M23) * num2;
                result.Z = 0.5f * num5;
                result.W = (matrix.M12 - matrix.M21) * num2;
            }

        }

        public static Vector3 ToEulerAngles2( Quaternion q)
        {
            // Store the Euler angles in radians
            Vector3 pitchYawRoll = new Vector3();

            double sqw = q.W * q.W;
            double sqx = q.X * q.X;
            double sqy = q.Y * q.Y;
            double sqz = q.Z * q.Z;

            // If quaternion is normalised the unit is one, otherwise it is the correction factor
            double unit = sqx + sqy + sqz + sqw;
            double test = q.X * q.Y + q.Z * q.W;

            if (test > 0.4999f * unit)                              // 0.4999f OR 0.5f - EPSILON
            {
                // Singularity at north pole
                pitchYawRoll.Y = 2f * (float)Math.Atan2(q.X, q.W);  // Yaw
                pitchYawRoll.X = PI * 0.5f;                         // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }
            else if (test < -0.4999f * unit)                        // -0.4999f OR -0.5f + EPSILON
            {
                // Singularity at south pole
                pitchYawRoll.Y = -2f * (float)Math.Atan2(q.X, q.W); // Yaw
                pitchYawRoll.X = -PI * 0.5f;                        // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }
            else
            {
                pitchYawRoll.Y = (float)Math.Atan2(2f * q.Y * q.W - 2f * q.X * q.Z, sqx - sqy - sqz + sqw);       // Yaw
                pitchYawRoll.X = (float)Math.Asin(2f * test / unit);                                             // Pitch
                pitchYawRoll.Z = (float)Math.Atan2(2f * q.X * q.W - 2f * q.Y * q.Z, -sqx + sqy - sqz + sqw);      // Roll
            }

            return pitchYawRoll;
        }

        private const double FaceRotationIncrementInDegrees = 5.0;


        public static Vector3 ExtractFaceRotation(Quaternion rotQuaternion)
        {
            double x = rotQuaternion.X;
            double y = rotQuaternion.Y;
            double z = rotQuaternion.Z;
            double w = rotQuaternion.W;

            // convert face rotation quaternion to Euler angles in degrees
            double yawD, pitchD, rollD;
            pitchD = Math.Atan2(2 * ((y * z) + (w * x)), (w * w) - (x * x) - (y * y) + (z * z)) / Math.PI * 180.0;
            yawD = Math.Asin(2 * ((w * y) - (x * z))) / Math.PI * 180.0;
            rollD = Math.Atan2(2 * ((x * y) + (w * z)), (w * w) + (x * x) - (y * y) - (z * z)) / Math.PI * 180.0;

            // clamp the values to a multiple of the specified increment to control the refresh rate
            double increment = FaceRotationIncrementInDegrees;
            double pitch = (int)(Math.Floor((pitchD + ((increment / 2.0) * (pitchD > 0 ? 1.0 : -1.0))) / increment) * increment);
            double yaw = (int)(Math.Floor((yawD + ((increment / 2.0) * (yawD > 0 ? 1.0 : -1.0))) / increment) * increment);
            double roll = (int)(Math.Floor((rollD + ((increment / 2.0) * (rollD > 0 ? 1.0 : -1.0))) / increment) * increment);

            Vector3 result = new Vector3(Math.PI * pitch / 180.0, Math.PI * yaw / 180.0, Math.PI * roll / 180.0);

            return result;
        }

        public static Quaternion CreateFromYawPitchRoll2(float yaw, float pitch, float roll)
        {
            float num = roll * 0.5f;
            float num2 = (float)Math.Sin((double)num);
            float num3 = (float)Math.Cos((double)num);
            float num4 = pitch * 0.5f;
            float num5 = (float)Math.Sin((double)num4);
            float num6 = (float)Math.Cos((double)num4);
            float num7 = yaw * 0.5f;
            float num8 = (float)Math.Sin((double)num7);
            float num9 = (float)Math.Cos((double)num7);
            Quaternion result;
            result.X = num9 * num5 * num3 + num8 * num6 * num2;
            result.Y = num8 * num6 * num3 - num9 * num5 * num2;
            result.Z = num9 * num6 * num2 - num8 * num5 * num3;
            result.W = num9 * num6 * num3 + num8 * num5 * num2;
            return result;
        }

        public static Quaternion CreateFromYawPitchRoll(double yaw, double pitch, double roll)
        {
            Quaternion quaternion;
            double num9 = roll * 0.5f;
            double num6 = (double)Math.Sin((double)num9);
            double num5 = (double)Math.Cos((double)num9);
            double num8 = pitch * 0.5f;
            double num4 = (double)Math.Sin((double)num8);
            double num3 = (double)Math.Cos((double)num8);
            double num7 = yaw * 0.5f;
            double num2 = (double)Math.Sin((double)num7);
            double num = (double)Math.Cos((double)num7);
            quaternion.X = ((num * num4) * num5) + ((num2 * num3) * num6);
            quaternion.Y = ((num2 * num3) * num5) - ((num * num4) * num6);
            quaternion.Z = ((num * num3) * num6) - ((num2 * num4) * num5);
            quaternion.W = ((num * num3) * num5) + ((num2 * num4) * num6);
            return quaternion;
        }

        public static void CreateFromYawPitchRoll(double yaw, double pitch, double roll, out Quaternion result)
        {
            double num9 = roll * 0.5f;
            double num6 = (double)Math.Sin((double)num9);
            double num5 = (double)Math.Cos((double)num9);
            double num8 = pitch * 0.5f;
            double num4 = (double)Math.Sin((double)num8);
            double num3 = (double)Math.Cos((double)num8);
            double num7 = yaw * 0.5f;
            double num2 = (double)Math.Sin((double)num7);
            double num = (double)Math.Cos((double)num7);
            result.X = ((num * num4) * num5) + ((num2 * num3) * num6);
            result.Y = ((num2 * num3) * num5) - ((num * num4) * num6);
            result.Z = ((num * num3) * num6) - ((num2 * num4) * num5);
            result.W = ((num * num3) * num5) + ((num2 * num4) * num6);
        }

        public static Quaternion Divide(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            double x = quaternion1.X;
            double y = quaternion1.Y;
            double z = quaternion1.Z;
            double w = quaternion1.W;
            double num14 = (((quaternion2.X * quaternion2.X) + (quaternion2.Y * quaternion2.Y)) + (quaternion2.Z * quaternion2.Z)) + (quaternion2.W * quaternion2.W);
            double num5 = 1f / num14;
            double num4 = -quaternion2.X * num5;
            double num3 = -quaternion2.Y * num5;
            double num2 = -quaternion2.Z * num5;
            double num = quaternion2.W * num5;
            double num13 = (y * num2) - (z * num3);
            double num12 = (z * num4) - (x * num2);
            double num11 = (x * num3) - (y * num4);
            double num10 = ((x * num4) + (y * num3)) + (z * num2);
            quaternion.X = ((x * num) + (num4 * w)) + num13;
            quaternion.Y = ((y * num) + (num3 * w)) + num12;
            quaternion.Z = ((z * num) + (num2 * w)) + num11;
            quaternion.W = (w * num) - num10;
            return quaternion;

        }

        public static void Divide(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
        {
            double x = quaternion1.X;
            double y = quaternion1.Y;
            double z = quaternion1.Z;
            double w = quaternion1.W;
            double num14 = (((quaternion2.X * quaternion2.X) + (quaternion2.Y * quaternion2.Y)) + (quaternion2.Z * quaternion2.Z)) + (quaternion2.W * quaternion2.W);
            double num5 = 1f / num14;
            double num4 = -quaternion2.X * num5;
            double num3 = -quaternion2.Y * num5;
            double num2 = -quaternion2.Z * num5;
            double num = quaternion2.W * num5;
            double num13 = (y * num2) - (z * num3);
            double num12 = (z * num4) - (x * num2);
            double num11 = (x * num3) - (y * num4);
            double num10 = ((x * num4) + (y * num3)) + (z * num2);
            result.X = ((x * num) + (num4 * w)) + num13;
            result.Y = ((y * num) + (num3 * w)) + num12;
            result.Z = ((z * num) + (num2 * w)) + num11;
            result.W = (w * num) - num10;

        }


        public static double Dot(Quaternion quaternion1, Quaternion quaternion2)
        {
            return ((((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W));
        }


        public static void Dot(ref Quaternion quaternion1, ref Quaternion quaternion2, out double result)
        {
            result = (((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W);
        }

        public Vector3 rotate_vector(Vector3 v)
        {
            Vector3 result = new Vector3(0, 0, 0);

            result.X = W * W * v.X + 2 * Y * W * v.Z - 2 * Z * W * v.Y + X * X * v.X + 2 * Y * X * v.Y + 2 * Z * X * v.Z - Z * Z * v.X - Y * Y * v.X;
            result.Y = 2 * X * Y * v.X + Y * Y * v.Y + 2 * Z * Y * v.Z + 2 * W * Z * v.X - Z * Z * v.Y + W * W * v.Y - 2 * X * W * v.Z - X * X * v.Y;
            result.Z = 2 * X * Z * v.X + 2 * Y * Z * v.Y + Z * Z * v.Z - 2 * W * Y * v.X - Y * Y * v.Z + 2 * W * X * v.Y - X * X * v.Z + W * W * v.Z;

            return result;
        }

        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Quaternion)
            {
                flag = this.Equals((Quaternion)obj);
            }
            return flag;
        }


        public bool Equals(Quaternion other)
        {
            return ((((this.X == other.X) && (this.Y == other.Y)) && (this.Z == other.Z)) && (this.W == other.W));
        }


        public override int GetHashCode()
        {
            return (((this.X.GetHashCode() + this.Y.GetHashCode()) + this.Z.GetHashCode()) + this.W.GetHashCode());
        }


        public static Quaternion Inverse(Quaternion quaternion)
        {
            Quaternion quaternion2;
            double num2 = (((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y)) + (quaternion.Z * quaternion.Z)) + (quaternion.W * quaternion.W);
            double num = 1f / num2;
            quaternion2.X = -quaternion.X * num;
            quaternion2.Y = -quaternion.Y * num;
            quaternion2.Z = -quaternion.Z * num;
            quaternion2.W = quaternion.W * num;
            return quaternion2;

        }

        public static void Inverse(ref Quaternion quaternion, out Quaternion result)
        {
            double num2 = (((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y)) + (quaternion.Z * quaternion.Z)) + (quaternion.W * quaternion.W);
            double num = 1f / num2;
            result.X = -quaternion.X * num;
            result.Y = -quaternion.Y * num;
            result.Z = -quaternion.Z * num;
            result.W = quaternion.W * num;
        }

        public double Length()
        {
            double num = (((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z)) + (this.W * this.W);
            return (double)Math.Sqrt((double)num);
        }


        public double LengthSquared()
        {
            return ((((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z)) + (this.W * this.W));
        }


        public static Quaternion Lerp(Quaternion quaternion1, Quaternion quaternion2, double amount)
        {
            double num = amount;
            double num2 = 1f - num;
            Quaternion quaternion = new Quaternion();
            double num5 = (((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W);
            if (num5 >= 0f)
            {
                quaternion.X = (num2 * quaternion1.X) + (num * quaternion2.X);
                quaternion.Y = (num2 * quaternion1.Y) + (num * quaternion2.Y);
                quaternion.Z = (num2 * quaternion1.Z) + (num * quaternion2.Z);
                quaternion.W = (num2 * quaternion1.W) + (num * quaternion2.W);
            }
            else
            {
                quaternion.X = (num2 * quaternion1.X) - (num * quaternion2.X);
                quaternion.Y = (num2 * quaternion1.Y) - (num * quaternion2.Y);
                quaternion.Z = (num2 * quaternion1.Z) - (num * quaternion2.Z);
                quaternion.W = (num2 * quaternion1.W) - (num * quaternion2.W);
            }
            double num4 = (((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y)) + (quaternion.Z * quaternion.Z)) + (quaternion.W * quaternion.W);
            double num3 = 1f / ((double)Math.Sqrt((double)num4));
            quaternion.X *= num3;
            quaternion.Y *= num3;
            quaternion.Z *= num3;
            quaternion.W *= num3;
            return quaternion;
        }


        public static void Lerp(ref Quaternion quaternion1, ref Quaternion quaternion2, double amount, out Quaternion result)
        {
            double num = amount;
            double num2 = 1f - num;
            double num5 = (((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W);
            if (num5 >= 0f)
            {
                result.X = (num2 * quaternion1.X) + (num * quaternion2.X);
                result.Y = (num2 * quaternion1.Y) + (num * quaternion2.Y);
                result.Z = (num2 * quaternion1.Z) + (num * quaternion2.Z);
                result.W = (num2 * quaternion1.W) + (num * quaternion2.W);
            }
            else
            {
                result.X = (num2 * quaternion1.X) - (num * quaternion2.X);
                result.Y = (num2 * quaternion1.Y) - (num * quaternion2.Y);
                result.Z = (num2 * quaternion1.Z) - (num * quaternion2.Z);
                result.W = (num2 * quaternion1.W) - (num * quaternion2.W);
            }
            double num4 = (((result.X * result.X) + (result.Y * result.Y)) + (result.Z * result.Z)) + (result.W * result.W);
            double num3 = 1f / ((double)Math.Sqrt((double)num4));
            result.X *= num3;
            result.Y *= num3;
            result.Z *= num3;
            result.W *= num3;

        }


        public static Quaternion Slerp(Quaternion quaternion1, Quaternion quaternion2, double amount)
        {
            double num2;
            double num3;
            Quaternion quaternion;
            double num = amount;
            double num4 = (((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W);
            bool flag = false;
            if (num4 < 0f)
            {
                flag = true;
                num4 = -num4;
            }
            if (num4 > 0.999999f)
            {
                num3 = 1f - num;
                num2 = flag ? -num : num;
            }
            else
            {
                double num5 = (double)Math.Acos((double)num4);
                double num6 = (double)(1.0 / Math.Sin((double)num5));
                num3 = ((double)Math.Sin((double)((1f - num) * num5))) * num6;
                num2 = flag ? (((double)-Math.Sin((double)(num * num5))) * num6) : (((double)Math.Sin((double)(num * num5))) * num6);
            }
            quaternion.X = (num3 * quaternion1.X) + (num2 * quaternion2.X);
            quaternion.Y = (num3 * quaternion1.Y) + (num2 * quaternion2.Y);
            quaternion.Z = (num3 * quaternion1.Z) + (num2 * quaternion2.Z);
            quaternion.W = (num3 * quaternion1.W) + (num2 * quaternion2.W);
            return quaternion;
        }


        public static void Slerp(ref Quaternion quaternion1, ref Quaternion quaternion2, double amount, out Quaternion result)
        {
            double num2;
            double num3;
            double num = amount;
            double num4 = (((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W);
            bool flag = false;
            if (num4 < 0f)
            {
                flag = true;
                num4 = -num4;
            }
            if (num4 > 0.999999f)
            {
                num3 = 1f - num;
                num2 = flag ? -num : num;
            }
            else
            {
                double num5 = (double)Math.Acos((double)num4);
                double num6 = (double)(1.0 / Math.Sin((double)num5));
                num3 = ((double)Math.Sin((double)((1f - num) * num5))) * num6;
                num2 = flag ? (((double)-Math.Sin((double)(num * num5))) * num6) : (((double)Math.Sin((double)(num * num5))) * num6);
            }
            result.X = (num3 * quaternion1.X) + (num2 * quaternion2.X);
            result.Y = (num3 * quaternion1.Y) + (num2 * quaternion2.Y);
            result.Z = (num3 * quaternion1.Z) + (num2 * quaternion2.Z);
            result.W = (num3 * quaternion1.W) + (num2 * quaternion2.W);
        }


        public static Quaternion Subtract(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            quaternion.X = quaternion1.X - quaternion2.X;
            quaternion.Y = quaternion1.Y - quaternion2.Y;
            quaternion.Z = quaternion1.Z - quaternion2.Z;
            quaternion.W = quaternion1.W - quaternion2.W;
            return quaternion;
        }


        public static void Subtract(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
        {
            result.X = quaternion1.X - quaternion2.X;
            result.Y = quaternion1.Y - quaternion2.Y;
            result.Z = quaternion1.Z - quaternion2.Z;
            result.W = quaternion1.W - quaternion2.W;
        }


        public static Quaternion Multiply(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            double x = quaternion1.X;
            double y = quaternion1.Y;
            double z = quaternion1.Z;
            double w = quaternion1.W;
            double num4 = quaternion2.X;
            double num3 = quaternion2.Y;
            double num2 = quaternion2.Z;
            double num = quaternion2.W;
            double num12 = (y * num2) - (z * num3);
            double num11 = (z * num4) - (x * num2);
            double num10 = (x * num3) - (y * num4);
            double num9 = ((x * num4) + (y * num3)) + (z * num2);
            quaternion.X = ((x * num) + (num4 * w)) + num12;
            quaternion.Y = ((y * num) + (num3 * w)) + num11;
            quaternion.Z = ((z * num) + (num2 * w)) + num10;
            quaternion.W = (w * num) - num9;
            return quaternion;
        }


        public static Quaternion Multiply(Quaternion quaternion1, double scaleFactor)
        {
            Quaternion quaternion;
            quaternion.X = quaternion1.X * scaleFactor;
            quaternion.Y = quaternion1.Y * scaleFactor;
            quaternion.Z = quaternion1.Z * scaleFactor;
            quaternion.W = quaternion1.W * scaleFactor;
            return quaternion;
        }


        public static void Multiply(ref Quaternion quaternion1, double scaleFactor, out Quaternion result)
        {
            result.X = quaternion1.X * scaleFactor;
            result.Y = quaternion1.Y * scaleFactor;
            result.Z = quaternion1.Z * scaleFactor;
            result.W = quaternion1.W * scaleFactor;
        }


        public static void Multiply(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
        {
            double x = quaternion1.X;
            double y = quaternion1.Y;
            double z = quaternion1.Z;
            double w = quaternion1.W;
            double num4 = quaternion2.X;
            double num3 = quaternion2.Y;
            double num2 = quaternion2.Z;
            double num = quaternion2.W;
            double num12 = (y * num2) - (z * num3);
            double num11 = (z * num4) - (x * num2);
            double num10 = (x * num3) - (y * num4);
            double num9 = ((x * num4) + (y * num3)) + (z * num2);
            result.X = ((x * num) + (num4 * w)) + num12;
            result.Y = ((y * num) + (num3 * w)) + num11;
            result.Z = ((z * num) + (num2 * w)) + num10;
            result.W = (w * num) - num9;
        }


        public static Quaternion Negate(Quaternion quaternion)
        {
            Quaternion quaternion2;
            quaternion2.X = -quaternion.X;
            quaternion2.Y = -quaternion.Y;
            quaternion2.Z = -quaternion.Z;
            quaternion2.W = -quaternion.W;
            return quaternion2;
        }


        public static void Negate(ref Quaternion quaternion, out Quaternion result)
        {
            result.X = -quaternion.X;
            result.Y = -quaternion.Y;
            result.Z = -quaternion.Z;
            result.W = -quaternion.W;
        }


        public void Normalize()
        {
            double num2 = (((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z)) + (this.W * this.W);
            double num = 1f / ((double)Math.Sqrt((double)num2));
            this.X *= num;
            this.Y *= num;
            this.Z *= num;
            this.W *= num;
        }


        public static Quaternion Normalize(Quaternion quaternion)
        {
            Quaternion quaternion2;
            double num2 = (((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y)) + (quaternion.Z * quaternion.Z)) + (quaternion.W * quaternion.W);
            double num = 1f / ((double)Math.Sqrt((double)num2));
            quaternion2.X = quaternion.X * num;
            quaternion2.Y = quaternion.Y * num;
            quaternion2.Z = quaternion.Z * num;
            quaternion2.W = quaternion.W * num;
            return quaternion2;
        }


        public static void Normalize(ref Quaternion quaternion, out Quaternion result)
        {
            double num2 = (((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y)) + (quaternion.Z * quaternion.Z)) + (quaternion.W * quaternion.W);
            double num = 1f / ((double)Math.Sqrt((double)num2));
            result.X = quaternion.X * num;
            result.Y = quaternion.Y * num;
            result.Z = quaternion.Z * num;
            result.W = quaternion.W * num;
        }


        public static Quaternion operator +(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            quaternion.X = quaternion1.X + quaternion2.X;
            quaternion.Y = quaternion1.Y + quaternion2.Y;
            quaternion.Z = quaternion1.Z + quaternion2.Z;
            quaternion.W = quaternion1.W + quaternion2.W;
            return quaternion;
        }


        public static Quaternion operator /(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            double x = quaternion1.X;
            double y = quaternion1.Y;
            double z = quaternion1.Z;
            double w = quaternion1.W;
            double num14 = (((quaternion2.X * quaternion2.X) + (quaternion2.Y * quaternion2.Y)) + (quaternion2.Z * quaternion2.Z)) + (quaternion2.W * quaternion2.W);
            double num5 = 1f / num14;
            double num4 = -quaternion2.X * num5;
            double num3 = -quaternion2.Y * num5;
            double num2 = -quaternion2.Z * num5;
            double num = quaternion2.W * num5;
            double num13 = (y * num2) - (z * num3);
            double num12 = (z * num4) - (x * num2);
            double num11 = (x * num3) - (y * num4);
            double num10 = ((x * num4) + (y * num3)) + (z * num2);
            quaternion.X = ((x * num) + (num4 * w)) + num13;
            quaternion.Y = ((y * num) + (num3 * w)) + num12;
            quaternion.Z = ((z * num) + (num2 * w)) + num11;
            quaternion.W = (w * num) - num10;
            return quaternion;
        }


        public static bool operator ==(Quaternion quaternion1, Quaternion quaternion2)
        {
            return ((((quaternion1.X == quaternion2.X) && (quaternion1.Y == quaternion2.Y)) && (quaternion1.Z == quaternion2.Z)) && (quaternion1.W == quaternion2.W));
        }


        public static bool operator !=(Quaternion quaternion1, Quaternion quaternion2)
        {
            if (((quaternion1.X == quaternion2.X) && (quaternion1.Y == quaternion2.Y)) && (quaternion1.Z == quaternion2.Z))
            {
                return (quaternion1.W != quaternion2.W);
            }
            return true;
        }


        public static Quaternion operator *(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            double x = quaternion1.X;
            double y = quaternion1.Y;
            double z = quaternion1.Z;
            double w = quaternion1.W;
            double num4 = quaternion2.X;
            double num3 = quaternion2.Y;
            double num2 = quaternion2.Z;
            double num = quaternion2.W;
            double num12 = (y * num2) - (z * num3);
            double num11 = (z * num4) - (x * num2);
            double num10 = (x * num3) - (y * num4);
            double num9 = ((x * num4) + (y * num3)) + (z * num2);
            quaternion.X = ((x * num) + (num4 * w)) + num12;
            quaternion.Y = ((y * num) + (num3 * w)) + num11;
            quaternion.Z = ((z * num) + (num2 * w)) + num10;
            quaternion.W = (w * num) - num9;
            return quaternion;
        }


        public static Quaternion operator *(Quaternion quaternion1, double scaleFactor)
        {
            Quaternion quaternion;
            quaternion.X = quaternion1.X * scaleFactor;
            quaternion.Y = quaternion1.Y * scaleFactor;
            quaternion.Z = quaternion1.Z * scaleFactor;
            quaternion.W = quaternion1.W * scaleFactor;
            return quaternion;
        }


        public static Quaternion operator -(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            quaternion.X = quaternion1.X - quaternion2.X;
            quaternion.Y = quaternion1.Y - quaternion2.Y;
            quaternion.Z = quaternion1.Z - quaternion2.Z;
            quaternion.W = quaternion1.W - quaternion2.W;
            return quaternion;

        }


        public static Quaternion operator -(Quaternion quaternion)
        {
            Quaternion quaternion2;
            quaternion2.X = -quaternion.X;
            quaternion2.Y = -quaternion.Y;
            quaternion2.Z = -quaternion.Z;
            quaternion2.W = -quaternion.W;
            return quaternion2;
        }


        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
			sb.Append("{W:");
			sb.Append(string.Format("{0:N4}", this.W));
			sb.Append(" X:");
            sb.Append( string.Format("{0:N4}",  this.X));
            sb.Append(" Y:");
            sb.Append(string.Format("{0:N4}", this.Y));
            sb.Append(" Z:");
            sb.Append(string.Format("{0:N4}", this.Z));
            sb.Append("}");
            return sb.ToString();
        }

        internal Matrix ToMatrix()
        {
            Matrix matrix = Matrix.Identity;
            ToMatrix(out matrix);
            return matrix;
        }

        internal void ToMatrix(out Matrix matrix)
        {
            Quaternion.ToMatrix(this, out matrix);
        }

        internal static void ToMatrix(Quaternion quaternion, out Matrix matrix)
        {

            // source -> http://content.gpwiki.org/index.php/OpenGL:Tutorials:Using_Quaternions_to_represent_rotation#Quaternion_to_Matrix
            double x2 = quaternion.X * quaternion.X;
            double y2 = quaternion.Y * quaternion.Y;
            double z2 = quaternion.Z * quaternion.Z;
            double xy = quaternion.X * quaternion.Y;
            double xz = quaternion.X * quaternion.Z;
            double yz = quaternion.Y * quaternion.Z;
            double wx = quaternion.W * quaternion.X;
            double wy = quaternion.W * quaternion.Y;
            double wz = quaternion.W * quaternion.Z;

            // This calculation would be a lot more complicated for non-unit length quaternions
            // Note: The constructor of Matrix4 expects the Matrix in column-major format like expected by
            //   OpenGL
            matrix.M11 = 1.0f - 2.0f * (y2 + z2);
            matrix.M12 = 2.0f * (xy - wz);
            matrix.M13 = 2.0f * (xz + wy);
            matrix.M14 = 0.0f;

            matrix.M21 = 2.0f * (xy + wz);
            matrix.M22 = 1.0f - 2.0f * (x2 + z2);
            matrix.M23 = 2.0f * (yz - wx);
            matrix.M24 = 0.0f;

            matrix.M31 = 2.0f * (xz - wy);
            matrix.M32 = 2.0f * (yz + wx);
            matrix.M33 = 1.0f - 2.0f * (x2 + y2);
            matrix.M34 = 0.0f;

            matrix.M41 = 2.0f * (xz - wy);
            matrix.M42 = 2.0f * (yz + wx);
            matrix.M43 = 1.0f - 2.0f * (x2 + y2);
            matrix.M44 = 0.0f;

            //return Matrix4( 1.0f - 2.0f * (y2 + z2), 2.0f * (xy - wz), 2.0f * (xz + wy), 0.0f,
            //        2.0f * (xy + wz), 1.0f - 2.0f * (x2 + z2), 2.0f * (yz - wx), 0.0f,
            //        2.0f * (xz - wy), 2.0f * (yz + wx), 1.0f - 2.0f * (x2 + y2), 0.0f,
            //        0.0f, 0.0f, 0.0f, 1.0f)
            //    }
        }


        internal static Vector3 ToEulerAngles(Quaternion q)
        {
            // Store the Euler angles in radians
            Vector3 pitchYawRoll = new Vector3();

            double sqw = q.W * q.W;
            double sqx = q.X * q.X;
            double sqy = q.Y * q.Y;
            double sqz = q.Z * q.Z;

            // If quaternion is normalised the unit is one, otherwise it is the correction factor
            double unit = sqx + sqy + sqz + sqw;
            double test = q.X * q.Y + q.Z * q.W;

            if (test > 0.4999f * unit)                              // 0.4999f OR 0.5f - EPSILON
            {
                // Singularity at north pole
                pitchYawRoll.Y = 2f * (float)Math.Atan2(q.X, q.W);  // Yaw
                pitchYawRoll.X = Math.PI * 0.5f;                         // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }
            else if (test < -0.4999f * unit)                        // -0.4999f OR -0.5f + EPSILON
            {
                // Singularity at south pole
                pitchYawRoll.Y = -2f * (float)Math.Atan2(q.X, q.W); // Yaw
                pitchYawRoll.X = -Math.PI * 0.5f;                        // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }
            else
            {
                pitchYawRoll.Y = (float)Math.Atan2(2f * q.Y * q.W - 2f * q.X * q.Z, sqx - sqy - sqz + sqw);       // Yaw
                pitchYawRoll.X = (float)Math.Asin(2f * test / unit);                                             // Pitch
                pitchYawRoll.Z = (float)Math.Atan2(2f * q.X * q.W - 2f * q.Y * q.Z, -sqx + sqy - sqz + sqw);      // Roll
            }

            return pitchYawRoll;
        }

    }
}
