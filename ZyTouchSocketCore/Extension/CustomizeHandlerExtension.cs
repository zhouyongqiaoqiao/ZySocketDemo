using System;
using System.Collections.Generic;
using System.Text;
using ZyLightTouchSocketCore.Interface;

namespace ZyLightTouchSocketCore.Extension
{
    internal class CustomizeHandlerExtension
    {
        public static ICustomizeHandler ToCustomizeHandler(IServiceCustomizeHandler serviceCustomizeHandler)
        {
            if (serviceCustomizeHandler == null) return null;
            return new CustomizeHandler(serviceCustomizeHandler);
        }

        private class CustomizeHandler : ICustomizeHandler
        {
            private IServiceCustomizeHandler serviceCustomizeHandler;

            public CustomizeHandler(IServiceCustomizeHandler serviceCustomizeHandler)
            {
                this.serviceCustomizeHandler = serviceCustomizeHandler;
            }
 
            public void HandleInformation(int informationType, byte[] info)
            {
                this.serviceCustomizeHandler.HandleInformation(null,informationType, info);
            }

            public byte[] HandleQuery(int informationType, byte[] info)
            {
               return this.serviceCustomizeHandler.HandleQuery(null, informationType, info);
            }
        }
    }
}
