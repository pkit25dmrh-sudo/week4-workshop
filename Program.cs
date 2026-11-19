using System.Text;

namespace CET2007_Week4_Workshop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Exercise1();
            Console.WriteLine();
            Exercise2();
            Console.WriteLine();
            Exercise3();
            Console.WriteLine();
            Exercise4();
            Console.WriteLine();
            Exercise5();
        }

        // Exercise 1: Handling Invalid Sensor Readings
        static void Exercise1()
        {
            Console.WriteLine("=== Exercise 1: Handling Invalid Sensor Readings ===");
            Console.WriteLine();

            string[] sensorNames = { "Temperature", "Humidity", "Motion" };
            string[] rawReadings = { "22.5", "bad_data", "-5.0" };

            SensorReading[] readings = new SensorReading[sensorNames.Length];

            for (int i = 0; i < sensorNames.Length; i++)
            {
                double value = 0.0;

                try
                {
                    value = Convert.ToDouble(rawReadings[i]);

                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(value), "Sensor reading cannot be negative.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid number format for " + sensorNames[i] + ". Setting value to 0.0.");
                    value = 0.0;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("Error for " + sensorNames[i] + ": " + ex.Message + " Setting value to 0.0.");
                    value = 0.0;
                }

                readings[i] = new SensorReading(sensorNames[i], value);
            }

            Console.WriteLine();
            Console.WriteLine("--- Final Sensor Readings ---");
            for (int i = 0; i < readings.Length; i++)
            {
                Console.WriteLine(readings[i].ToString());
            }

            Console.WriteLine();
            Console.WriteLine("Program finished successfully.");
        }

        // Exercise 2: Custom Exceptions for Faulty Sensors
        static void Exercise2()
        {
            Console.WriteLine("=== Exercise 2: Custom Exceptions for Faulty Sensors ===");
            Console.WriteLine();

            Sensor[] sensors =
            {
                new Sensor("Temperature", -20, 50, true),
                new Sensor("Humidity", 0, 100, true),
                new Sensor("Motion", 0, 1, false)
            };

            double[] SampleReading = { 22.5, 120.0, 1.0 };

            Console.WriteLine("Configured Sensors:");
            for (int i = 0; i < sensors.Length; i++)
            {
                Console.WriteLine(sensors[i]);
            }

            Console.WriteLine();
            Console.WriteLine("--- Validating Sample Readings ---");

            for (int i = 0; i < sensors.Length; i++)
            {
                Console.WriteLine();
                Console.WriteLine("Checking " + sensors[i].Name + " with value " + SampleReading[i] + " ...");

                try
                {
                    sensors[i].ValidateReading(SampleReading[i]);
                    Console.WriteLine(sensors[i].Name + " reading accepted.");
                }
                catch (SensorOfflineException)
                {
                    Console.WriteLine(sensors[i].Name + " sensor offline – check connection.");
                }
                catch (ReadingOutOfRangeException)
                {
                    Console.WriteLine(sensors[i].Name + " reading out of range (value " + SampleReading[i] + ").");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Done.");
        }

        // Exercise 3: Demonstrating Exception Hierarchy
        static void Exercise3()
        {
            Console.WriteLine("=== Exercise 3: Exception Hierarchy Demo ===");
            Console.WriteLine();

            Sensor[] sensors =
            {
                new Sensor("Temperature", -20, 50, true),
                new Sensor("Humidity", 0, 100, true),
                new Sensor("Motion", 0, 1, true)
            };

            Console.WriteLine("Configured Sensors:");
            for (int i = 0; i < sensors.Length; i++)
            {
                Console.WriteLine(sensors[i]);
            }

            Console.WriteLine();
            Console.WriteLine("--- Demonstrating Exception Handling Order ---");
            Console.WriteLine();

            try
            {
                // (a) ReadingOutOfRangeException
                Console.WriteLine("Testing extreme reading for Temperature...");
                sensors[0].ValidateReading(999);

                // (b) SensorOfflineException
                Console.WriteLine("Turning off Humidity sensor...");
                sensors[1].IsOnline = false;
                sensors[1].ValidateReading(50);

                // (c) IndexOutOfRangeException
                Console.WriteLine("Accessing invalid sensor index...");
                sensors[5].ValidateReading(10);
            }
            catch (ReadingOutOfRangeException ex)
            {
                Console.WriteLine("[Caught] " + ex.GetType().Name + ": " + ex.Message);
            }
            catch (SensorOfflineException ex)
            {
                Console.WriteLine("[Caught] " + ex.GetType().Name + ": " + ex.Message);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("[Caught] " + ex.GetType().Name + ": " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Caught] " + ex.GetType().Name + ": " + ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Execution continues normally after handling exceptions.");
        }

        // Exercise 4: Adding Context to Custom Exceptions
        static void Exercise4()
        {
            Console.WriteLine("=== Exercise 4: Context-Rich Custom Exceptions ===");
            Console.WriteLine();

            Sensor[] sensors =
            {
                new Sensor("Humidity", 0, 100, true),
                new Sensor("Motion", 0, 1, false)
            };

            Console.WriteLine("Configured Sensors:");
            for (int i = 0; i < sensors.Length; i++)
            {
                Console.WriteLine(sensors[i]);
            }

            Console.WriteLine();
            Console.WriteLine("--- Simulating Exceptions with Context ---");
            Console.WriteLine();

            try
            {
                Console.WriteLine("Checking " + sensors[0].Name + " with value 120 ...");
                sensors[0].ValidateReading(120);
            }
            catch (ReadingOutOfRangeException ex)
            {
                Console.WriteLine("[Caught] " + ex.GetType().Name + ": " + ex.Message);
                Console.WriteLine("Details -> Attempted: " + ex.AttemptedValue + ", Min: " + ex.Min + ", Max: " + ex.Max);
            }
        }

        // Exercise 5: Monitoring Cycle with Finally
        static void Exercise5()
        {
            Console.WriteLine("=== Exercise 5: Monitoring Cycle ===");
            Console.WriteLine();

            Sensor[] sensors =
            {
                new Sensor("Temperature", -20, 50, true),
                new Sensor("Humidity", 0, 100, true),
                new Sensor("Motion", 0, 1, false),
                new Sensor("Pressure", 950, 1050, true),
                new Sensor("Light", 0, 1000, true)
            };

            double[] readings = { 25, 120, 1, 900, 500 };

            for (int i = 0; i < sensors.Length; i++)
            {
                Console.WriteLine(sensors[i]);

                try
                {
                    sensors[i].ValidateReading(readings[i]);
                    Console.WriteLine("OK: " + sensors[i].Name + " reading accepted: " + readings[i]);
                }
                catch (SensorOfflineException ex)
                {
                    Console.WriteLine("[SensorOfflineException] " + ex.Message);
                }
                catch (ReadingOutOfRangeException ex)
                {
                    Console.WriteLine("[ReadingOutOfRangeException] " + ex.Message);
                    Console.WriteLine("Attempted: " + ex.AttemptedValue + ", Min: " + ex.Min + ", Max: " + ex.Max);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("[ArgumentOutOfRangeException] " + ex.Message);
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine("[IndexOutOfRangeException] " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[General Exception] " + ex.Message);
                }
                finally
                {
                    Console.WriteLine();
                    Console.WriteLine(sensors[i].Name + " Sensor Monitoring pass complete");
                    Console.WriteLine();
                }
            }
        }
    }
}
