using System;
using System.Threading.Tasks;
using ZySocketCore.Client;

namespace ZySocketCore.Core.CustomizeInfo.Client
{
    internal class CustomizeOutter : ICustomizeOutter
    {
        private readonly ZyClientEngine clientEngine;
        public CustomizeOutter(ZyClientEngine engine)
        {
            this.clientEngine = engine;
        }

        public byte[] Query(int informationType, byte[] info)
        {
            var data = this.clientEngine.Queryer.QueryCustomMessage(this.clientEngine.CurrentUserID, SystemSettings.ServerDefaultId, informationType, info);
            return data; //Task.FromResult(data);
        }

        //该方法现在已出现异常卡死，原因未知，暂时注释掉
        public Task<byte[]> QueryAsync(int informationType, byte[] info)
        {
           var data = this.clientEngine.Queryer.QueryCustomMessageAsync(this.clientEngine.CurrentUserID, SystemSettings.ServerDefaultId, informationType, info);
            return  data; //Task.FromResult(data);
        }

        public Task SendAsync(int informationType, byte[] info)
        {
            return this.SendAsync(SystemSettings.ServerDefaultId, informationType, info);
        }

        public Task SendAsync(string targetUserID, int informationType, byte[] info)
        {
            return this.clientEngine.SendCustomMessageAsync(this.clientEngine.CurrentUserID, targetUserID, informationType, info);
        }

        public void SendCertainly(string targetUserID, int informationType, byte[] info)
        {
            this.clientEngine.Queryer.SendCertainlyCustomMessage(this.clientEngine.CurrentUserID, targetUserID, informationType, info);
        }
    }
}
