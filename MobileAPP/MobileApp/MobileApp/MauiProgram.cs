// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MauiProgram.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp
{
    #region Imports

    using System.Reflection;

    using CommunityToolkit.Maui;

    using epj.Expander.Maui;

    using MobileApp.Interfaces;
    using MobileApp.Services;

    #endregion

    public static class MauiProgram
    {
        #region Public Methods and Operators

        public static MauiApp CreateMauiApp()
        {
            Expander.EnableAnimations();

            return MauiApp.CreateBuilder()
                .UseMauiApp<BargainScanApp>()
                .UseMauiCommunityToolkit()
                .RegisterServices()
                .RegisterViewModelBindings()
                .UseExpander()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .Build();
        }

        #endregion

        #region Private Methods

        private static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<IDataService, DataService>();
            mauiAppBuilder.Services.AddSingleton<IEventAggregator, EventAggregator>();
            mauiAppBuilder.Services.AddSingleton<IResolverService, ResolverService>();

            return mauiAppBuilder;
        }

        private static MauiAppBuilder RegisterViewModelBindings(this MauiAppBuilder mauiAppBuilder)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            List<Type> classes = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract).ToList();
            List<Type> views = classes.FindAll(c => c.Name.EndsWith("View"));

            foreach (Type view in views)
            {
                if (classes.Find(c => c.Name == view.Name + "Model") is not { } viewModel)
                {
                    continue;
                }

                mauiAppBuilder.Services.AddSingleton(view, serviceProvider => CreateView(view, viewModel, serviceProvider));
            }

            return mauiAppBuilder;
        }

        private static object CreateView(Type view, Type viewModel, IServiceProvider serviceProvider)
        {
            if (ActivatorUtilities.CreateInstance(serviceProvider, view) is not BindableObject viewInstance)
            {
                return null;
            }

            viewInstance.BindingContext = ActivatorUtilities.CreateInstance(serviceProvider, viewModel);
            return viewInstance;
        }

        #endregion
    }

    public interface IResolverService
    {
        #region Public Methods and Operators

        object Resolve<TView>() where TView : class;

        #endregion
    }

    public class ResolverService : IResolverService
    {
        #region Constants and Private Fields

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Constructors and Destructors

        public ResolverService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Public Methods and Operators

        public object Resolve<TView>() where TView : class
        {
            return _serviceProvider.GetService(typeof(TView));
        }

        #endregion
    }
}