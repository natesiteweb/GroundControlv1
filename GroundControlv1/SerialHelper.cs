using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundControlv1
{
    class SerialHelper
    {
        public static SerialPort serialPort = new SerialPort();

        public static Int16 ReadInt16()
        {
            byte[] bytes = new byte[2];
            bytes[0] = (byte)serialPort.ReadByte();
            bytes[1] = (byte)serialPort.ReadByte();

            return BitConverter.ToInt16(bytes, 0);
        }

        public static float ReadFloat()
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)serialPort.ReadByte();
            bytes[1] = (byte)serialPort.ReadByte();
            bytes[2] = (byte)serialPort.ReadByte();
            bytes[3] = (byte)serialPort.ReadByte();

            return BitConverter.ToSingle(bytes, 0);
        }

        public static Int32 ReadInt32()
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)serialPort.ReadByte();
            bytes[1] = (byte)serialPort.ReadByte();
            bytes[2] = (byte)serialPort.ReadByte();
            bytes[3] = (byte)serialPort.ReadByte();

            return BitConverter.ToInt32(bytes, 0);
        }

        public static UInt32 ReadUInt32()
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)serialPort.ReadByte();
            bytes[1] = (byte)serialPort.ReadByte();
            bytes[2] = (byte)serialPort.ReadByte();
            bytes[3] = (byte)serialPort.ReadByte();

            return BitConverter.ToUInt32(bytes, 0);
        }

        public static void Send(float num)
        {
            byte[] p = new byte[4];
            System.Buffer.BlockCopy(new float[1] { num }, 0, p, 0, 4);

            serialPort.Write(p, 0, 4);
        }

        public static void Send(Int32 num)
        {
            byte[] p = new byte[4];
            System.Buffer.BlockCopy(new Int32[1] { num }, 0, p, 0, 4);

            serialPort.Write(p, 0, 4);
        }

        public static void Send(UInt32 num)
        {
            byte[] p = new byte[4];
            System.Buffer.BlockCopy(new UInt32[1] { num }, 0, p, 0, 4);

            serialPort.Write(p, 0, 4);
        }

        public static void Send(Int16 num)
        {
            byte[] p = new byte[2];
            System.Buffer.BlockCopy(new Int16[1] { num }, 0, p, 0, 2);

            serialPort.Write(p, 0, 2);
        }

        public static void SetPacketID(byte id)
        {
            serialPort.Write(new byte[1] { id }, 0, 1);
        }

        public enum CommandFromSerial : byte
        {
            //From STM to leo, from leo to Ground Control
            GYRO_PACKET = 0x01,
            PID_OUTPUT_PACKET = 0x02,
            PID_GAIN_FIRST_PACKET = 0x03,
            PID_GAIN_SECOND_PACKET = 0x04,
            CHECK_GROUND_REQUEST = 0x05,

            //From leo to STM, from Ground Control to leo
            GYRO_REQUEST = 0xF1,
            PID_OUTPUT_REQUEST = 0xF2,
            PID_GAIN_FIRST_REQUEST = 0xF3,
            PID_GAIN_SECOND_REQUEST = 0xF4,
            PID_GAIN_FIRST_UPDATE_REQUEST = 0xF5,
            PID_GAIN_SECOND_UPDATE_REQUEST = 0xF6,
            CALIBRATE_REQUEST = 0xF7
        }
    }
}
