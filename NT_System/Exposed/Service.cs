using System;
using System.ServiceProcess;

namespace BSS.System.Service
{
    ///<summary>Interact with Windows services</summary>
    public static partial class xService
    {
        ///<summary>Returns the status of a given service.</summary>
        public static String GetStatusString(String serviceName)
        {
            using ServiceController sc = new(serviceName);

            return sc.Status switch
            {
                ServiceControllerStatus.Running => "running",
                ServiceControllerStatus.Stopped => "stopped",
                ServiceControllerStatus.Paused => "paused",
                ServiceControllerStatus.StopPending => "stopping",
                ServiceControllerStatus.StartPending => "starting",
                _ => "status changing",
            };
        }

        ///<summary>Returns the status of a given service.</summary>
        public static ServiceControllerStatus GetStatusEnum(String serviceName)
        {
            return new ServiceController(serviceName).Status;
        }

        ///<summary>Starts a given service</summary>
        public static void Start(String serviceName, String[] args = null)
        {
            using ServiceController sc = new(serviceName);

            if (args != null)
            {
                sc.Start(args);

                return;
            }

            sc.Start();
        }

        ///<summary>Stops a given service</summary>
        public static void Stop(String serviceName)
        {
            using ServiceController sc = new(serviceName);

            sc.Stop();
        }

        ///<summary>Stops a given service</summary>
        public static void Pause(String serviceName)
        {
            using ServiceController sc = new(serviceName);

            sc.Pause();
        }

        ///<summary>Changes the service start type</summary>
        public static void SetStartupType(String serviceName, ServiceStartMode newMode)
        {
            using ServiceController sc = new(serviceName);

            ChangeStartMode(sc, newMode);
        }

        ///<summary>Gets the service start type</summary>
        public static String GetStartupType(String serviceName)
        {
            using ServiceController sc = new(serviceName);

            return sc.StartType.ToString();
        }
    }
}