﻿using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.IO;
using HearthStone.Server;
using HearthStone.Library;

namespace HearthStone.PhotonServerEnvironment
{
    public class PhotonServerApplication : ApplicationBase
    {
        public static PhotonServerApplication ServerInstance { get; private set; }
        public static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new PhotonServerPeer(initRequest);
        }

        protected override void Setup()
        {
            ServerInstance = this;
            SetupServices();
            SetupFactories();
            LogService.Info("Server Setup Successful!");
        }

        protected override void TearDown()
        {

        }

        private void SetupServices()
        {
            SetupLog();
        }
        private void SetupFactories()
        {
            EndPointFactory.Initial();
        }

        private void SetupLog()
        {
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(ApplicationPath, "log");
            FileInfo file = new FileInfo(Path.Combine(BinaryPath, "log4net.config"));
            if (file.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);
            }
            LogService.InitialService(Log.Info, Log.InfoFormat, Log.Error, Log.ErrorFormat, Log.Fatal, Log.FatalFormat);
        }
    }
}
