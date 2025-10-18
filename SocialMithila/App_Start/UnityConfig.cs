using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using SocialMithila.Business.Business;
using SocialMithila.Business.Interface;
using System;
using System.Web.Mvc;

namespace SocialMithila.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<IBllLogin, BllLogin>();
            container.RegisterType<IBllHome, BllHome>();
            container.RegisterType<IBllUserProfile, BllUserProfile>();
            container.RegisterType<IBllBusiness, BllBusiness>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
