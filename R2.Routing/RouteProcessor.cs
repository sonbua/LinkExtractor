using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace R2.Routing
{
    public class RouteProcessor : IRouteProcessor
    {
        private readonly CommandRouteTable _commandRouteTable;
        private readonly QueryRouteTable _queryRouteTable;
        private readonly IRequestProcessor _requestProcessor;

        public RouteProcessor(
            IRequestProcessor requestProcessor,
            CommandRouteTable commandRouteTable,
            QueryRouteTable queryRouteTable)
        {
            _requestProcessor = requestProcessor;
            _commandRouteTable = commandRouteTable;
            _queryRouteTable = queryRouteTable;
        }

        public async Task ProcessCommandAsync(string commandName, string commandObjectString)
        {
            var routeEntry = _commandRouteTable.Table[commandName.ToLower()];
            var commandObject = JsonConvert.DeserializeObject(commandObjectString, routeEntry.RequestType);

            await _requestProcessor.ProcessCommandAsync(commandObject, routeEntry.HandlerType);
        }

        public async Task<object> ProcessQueryAsync(string queryName, string queryObjectString)
        {
            var routeEntry = _queryRouteTable.Table[queryName.ToLower()];
            var queryObject = JsonConvert.DeserializeObject(queryObjectString, routeEntry.RequestType);

            return await _requestProcessor.ProcessQueryAsync(queryObject, routeEntry.HandlerType);
        }

        public async Task<object> ProcessUploadAsync(string uploadName, IList<IFile> files)
        {
            var routeEntry = _queryRouteTable.Table[uploadName.ToLower()];
            var uploadObject = Activator.CreateInstance(routeEntry.RequestType);

            RequestUtils.AttachFileToObject(uploadObject, files);

            return await _requestProcessor.ProcessQueryAsync(uploadObject, routeEntry.HandlerType);
        }
    }
}