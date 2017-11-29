using System;
using System.Diagnostics;
using System.Security.Cryptography;
/*
 * Nov 29, 2017
 * For RSA assignment.
 * Hyukpyo Hong
 */
namespace RSASample
{
    class Program
    {
        static void Main(string[] args)
        {
            RSA r = new RSA();
            r.Echo("Start RSA Test");

            r.AskSize();
            r.line();

            r.GenerateKey();
            r.line();

            r.AskPlainText();
            r.line();

            r.Encrypt();
            r.line();

            r.Decrypt();
            r.line();

            r.Echo("Enter any key to Exit");
            Console.ReadKey();
        }
    }

    public class RSA
    {
        int size;
        string plain;
        RSACryptoServiceProvider csp;
        Byte[] bytesPlainText;
        Byte[] bytesCypherText;
        Stopwatch sw;

        internal void AskSize()
        {
            while (true)
            {
                if (Int32.TryParse(Input("Input bit size to generate RSA key pair (512 - 4096)"), out size))
                {
                    if (size >= 512 && size <= 4096)
                    {
                        break;
                    }
                    else
                    {
                        Warn("Invalid Number. Please input Again.");
                    }
                }
                else
                {
                    Warn("Invalid Number. Please input Again.");
                }
            }
        }

        internal void AskPlainText()
        {
            while (true)
            {
                plain = Input("Input your Plain Text Less than " + (((size - 384) / 8) + 37) + " bits.");
                bytesPlainText = System.Text.Encoding.Unicode.GetBytes(plain);
                int len = bytesPlainText.Length;
                if ((((size - 384) / 8) + 37) < len)
                {
                    Warn("Your Plain Text is too long to encrypt");

                }
                else
                {
                    Info("Your Input Size is " + len + "bits. Good.");
                    break;
                }
            }
        }

        internal void GenerateKey()
        {
            Start();
            csp = new RSACryptoServiceProvider(size);
            var Key = csp.ExportParameters(true);
            var pubKey = csp.ExportParameters(false);

            Echo("Generated Public Key (e,n)");
            Info(BitConverter.ToInt16(Key.Exponent, 0) + ", " + BitConverter.ToInt32(Key.Modulus, 0));

            Echo("Public Key in XML Style is as below,");

            var sw = new System.IO.StringWriter();
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, pubKey);
            Info(sw.ToString());

            Echo("Generated Private Key d");
            Info(BitConverter.ToInt32(Key.D, 0));
            Stop();

        }
        internal void Encrypt()
        {
            Start();
            Echo("Start Encryption..");
            bytesCypherText = csp.Encrypt(bytesPlainText, false);
            Echo("Your Ciphertext is,");
            Info(System.Text.Encoding.UTF8.GetString(bytesCypherText));
            Stop();
        }

        internal void Decrypt()
        {
            Start();
            Echo("Start Decryption..");
            Byte[] bytesPlainTextData = csp.Decrypt(bytesCypherText, false);
            Echo("Your Original text is,");
            Info(System.Text.Encoding.Unicode.GetString(bytesPlainTextData));
            Stop();
        }

        public void Echo(string msg)
        {
            try
            {
                ConsoleColor prevfg = Console.ForegroundColor;
                ConsoleColor prevbg = Console.BackgroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine(msg);
                Console.ForegroundColor = prevfg;
                Console.BackgroundColor = prevbg;
            }
            catch (Exception e)
            {
                Warn("Error");
                Warn(e.ToString());
            }
        }
        public void Info(object msg)
        {
            try
            {
                ConsoleColor prevfg = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(msg.ToString());
                Console.ForegroundColor = prevfg;
            }
            catch (Exception e)
            {
                Warn("Error");
                Warn(e.ToString());
            }
        }

        public void Warn(object msg)
        {
            try
            {
                ConsoleColor prevfg = Console.ForegroundColor;
                ConsoleColor prevbg = Console.BackgroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(msg.ToString());
                Console.ForegroundColor = prevfg;
                Console.BackgroundColor = prevbg;
            }
            catch (Exception e)
            {
                Warn("Error");
                Warn(e.ToString());
            }
        }

        public string Input(object msg)
        {
            try
            {
                Echo(msg.ToString());
                return Console.ReadLine();
            }
            catch (Exception e)
            {
                Warn("Error");
                Warn(e.ToString());
            }
            return null;
        }

        internal void Start()
        {
            sw = Stopwatch.StartNew();
        }

        internal void Stop()
        {
            sw.Stop();
            Echo(string.Format("Time taken: {0}ms", sw.Elapsed.TotalMilliseconds));
        }

        internal void line()
        {
            Console.WriteLine();
            Console.WriteLine("--------------------------------------");
            Console.WriteLine();

        }
    }

}
