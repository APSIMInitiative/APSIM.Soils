﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace APSIM.Soils.Portal.ApsoilManageServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ApsoilManageServiceReference.IApsoilManage")]
    public interface IApsoilManage {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IApsoilManage/RefreshDBTablesFromXML", ReplyAction="http://tempuri.org/IApsoilManage/RefreshDBTablesFromXMLResponse")]
        string RefreshDBTablesFromXML();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IApsoilManage/CleanOutDBTables", ReplyAction="http://tempuri.org/IApsoilManage/CleanOutDBTablesResponse")]
        string CleanOutDBTables();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IApsoilManageChannel : APSIM.Soils.Portal.ApsoilManageServiceReference.IApsoilManage, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ApsoilManageClient : System.ServiceModel.ClientBase<APSIM.Soils.Portal.ApsoilManageServiceReference.IApsoilManage>, APSIM.Soils.Portal.ApsoilManageServiceReference.IApsoilManage {
        
        public ApsoilManageClient() {
        }
        
        public ApsoilManageClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ApsoilManageClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ApsoilManageClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ApsoilManageClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string RefreshDBTablesFromXML() {
            return base.Channel.RefreshDBTablesFromXML();
        }
        
        public string CleanOutDBTables() {
            return base.Channel.CleanOutDBTables();
        }
    }
}