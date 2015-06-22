﻿using log4net;
using System;
using DatabaseBenchmark.Exceptions;
using DatabaseBenchmark.Properties;

namespace DatabaseBenchmark.Benchmarking
{
    /// <summary>
    /// Represents a benchmark test suite that executes all of the tests.
    /// </summary>
    public class BenchmarkSuite
    {
        private ILog Logger;

        public event Action<BenchmarkTest, TestMethod> OnTestFinish;
        public BenchmarkTest Current { get; private set; }

        public BenchmarkSuite()
        {
            Logger = LogManager.GetLogger(Settings.Default.TestLogger);
        }

        public void ExecuteInit(BenchmarkTest test)
        {
            Current = test;
            string databaseName = test.Database.Name;

            try
            {
                Logger.Info("");
                Logger.Info(String.Format("{0} Init() started...", databaseName));

                Current.Init();

                Logger.Info(String.Format("{0} Init() ended...", databaseName));
            }
            catch (Exception exc)
            {
                Logger.Error(String.Format("{0} Init()", databaseName), exc);
                Logger.Error(String.Format("{0} Init() failed...", databaseName));
            }
            finally
            {
                Current = null;
            }
        }

        public void ExecuteWrite(BenchmarkTest test)
        {
            Current = test;
            string databaseName = test.Database.Name;

            try
            {
                Logger.Info(String.Format("{0} Write() started...", databaseName));
                Current.Write();
                Logger.Info(String.Format("{0} Write() ended...", databaseName));
            }
            catch (Exception exc)
            {
                Logger.Error(String.Format("{0} Write()", databaseName), exc);
                Logger.Error(String.Format("{0} Write() failed...", databaseName));
            }
            finally
            {
                Current = null;

                if(OnTestFinish != null)
                    OnTestFinish(test, TestMethod.Write);
            }
        }

        public void ExecuteRead(BenchmarkTest test)
        {
            Current = test;
            string databaseName = test.Database.Name;

            try
            {
                Logger.Info(String.Format("{0} Read() started...", databaseName));
                Current.Read();
                Logger.Info(String.Format("Records read: {0}", test.RecordsRead.ToString("N0")));
                Logger.Info(String.Format("{0} Read() ended...", databaseName));
            }
            catch(KeysNotOrderedException exc)
            {
                Logger.Error(String.Format("{0} {1}", databaseName, exc.Message));
                Logger.Info(String.Format("{0} The database does not return the records ordered by key. The test is invalid!...", databaseName));
            }
            catch (Exception exc)
            {
                Logger.Error(String.Format("{0} Read()", databaseName), exc);
                Logger.Error(String.Format("{0} Read() failed...", databaseName));
            }
            finally
            {
                Current = null;

                if(OnTestFinish != null)
                    OnTestFinish(test, TestMethod.Read);
            }
        }

        public void ExecuteSecondaryRead(BenchmarkTest test)
        {
            Current = test;
            string databaseName = test.Database.Name;

            try
            {
                Logger.Info(String.Format("{0} SecondaryRead() started...", databaseName));
                Current.SecondaryRead();
                Logger.Info(String.Format("Records read: {0}", test.RecordsRead.ToString("N0")));
                Logger.Info(String.Format("{0} SecondaryRead() ended...", databaseName));
            }
            catch (KeysNotOrderedException exc)
            {
                Logger.Error(String.Format("{0} Read()", databaseName), exc);
                Logger.Error(String.Format("{0} The database does not return the records ordered by key. The test is invalid!...", databaseName));
            }
            catch (Exception exc)
            {
                Logger.Error(String.Format("{0} Secondary Read()", databaseName), exc);
                Logger.Error(String.Format("{0} Secondary Read failed...", databaseName));
            }
            finally
            {
                Current = null;

                if(OnTestFinish != null)
                    OnTestFinish(test, TestMethod.SecondaryRead);
            }
        }

        public void ExecuteFinish(BenchmarkTest test)
        {
            Current = test;

            try
            {
                Current.Finish();
            }
            catch (Exception exc)
            {
                Logger.Error(String.Format("{0} Finish()", test.Database.Name), exc);
                Logger.Error(String.Format("{0} Finish() failed...", test.Database.Name));
            }
            finally
            {
                Current = null;
            }
        }
    }
}
