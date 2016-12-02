using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Test_Solution1.ServiceAgents.Installers;
using System;
using System.Windows.Forms;


namespace Windows
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {

            var container = Container.Initialize();

            container.Register(
                Castle.MicroKernel.Registration.Classes.FromThisAssembly()
                    .BasedOn<Form>());

            container.Install(FromAssembly.Containing(typeof(ServiceAgentInstaller)));

            Container.CastleContainer = container;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            

        }
    }
    public static class Container
    {
        public static IWindsorContainer CastleContainer { get; set; }

        internal static IWindsorContainer Initialize()
        {
            CastleContainer = new WindsorContainer();
            return CastleContainer;
        }

    }
}
