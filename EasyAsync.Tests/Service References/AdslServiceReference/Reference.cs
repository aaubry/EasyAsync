// 	Copyright 2011 Antoine Aubry
// 
// 	This file is part of EasyAsync.
// 
// 	EasyAsync is free software: you can redistribute it and/or modify
// 	it under the terms of the GNU Lesser General Public License as published by
// 	the Free Software Foundation, either version 3 of the License, or
// 	(at your option) any later version.
// 
// 	Foobar is distributed in the hope that it will be useful,
// 	but WITHOUT ANY WARRANTY; without even the implied warranty of
// 	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// 	GNU General Public License for more details.
// 
// 	You should have received a copy of the GNU General Public License
// 	along with EasyAsync. If not, see <http://www.gnu.org/licenses/>.
namespace EasyAsync.Tests.AdslServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://netservices.sapo.pt/definitions", ConfigurationName="AdslServiceReference.ADSLSoap")]
    public interface ADSLSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://netservices.sapo.pt/definitions/HasCoverage", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        bool HasCoverage(string telephoneNumber);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://netservices.sapo.pt/definitions/HasCoverage", ReplyAction="*")]
        System.IAsyncResult BeginHasCoverage(string telephoneNumber, System.AsyncCallback callback, object asyncState);
        
        bool EndHasCoverage(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ADSLSoapChannel : EasyAsync.Tests.AdslServiceReference.ADSLSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class HasCoverageCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public HasCoverageCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public bool Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ADSLSoapClient : System.ServiceModel.ClientBase<EasyAsync.Tests.AdslServiceReference.ADSLSoap>, EasyAsync.Tests.AdslServiceReference.ADSLSoap {
        
        private BeginOperationDelegate onBeginHasCoverageDelegate;
        
        private EndOperationDelegate onEndHasCoverageDelegate;
        
        private System.Threading.SendOrPostCallback onHasCoverageCompletedDelegate;
        
        public ADSLSoapClient() {
        }
        
        public ADSLSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ADSLSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ADSLSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ADSLSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<HasCoverageCompletedEventArgs> HasCoverageCompleted;
        
        public bool HasCoverage(string telephoneNumber) {
            return base.Channel.HasCoverage(telephoneNumber);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginHasCoverage(string telephoneNumber, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginHasCoverage(telephoneNumber, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public bool EndHasCoverage(System.IAsyncResult result) {
            return base.Channel.EndHasCoverage(result);
        }
        
        private System.IAsyncResult OnBeginHasCoverage(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string telephoneNumber = ((string)(inValues[0]));
            return this.BeginHasCoverage(telephoneNumber, callback, asyncState);
        }
        
        private object[] OnEndHasCoverage(System.IAsyncResult result) {
            bool retVal = this.EndHasCoverage(result);
            return new object[] {
                    retVal};
        }
        
        private void OnHasCoverageCompleted(object state) {
            if ((this.HasCoverageCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.HasCoverageCompleted(this, new HasCoverageCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void HasCoverageAsync(string telephoneNumber) {
            this.HasCoverageAsync(telephoneNumber, null);
        }
        
        public void HasCoverageAsync(string telephoneNumber, object userState) {
            if ((this.onBeginHasCoverageDelegate == null)) {
                this.onBeginHasCoverageDelegate = new BeginOperationDelegate(this.OnBeginHasCoverage);
            }
            if ((this.onEndHasCoverageDelegate == null)) {
                this.onEndHasCoverageDelegate = new EndOperationDelegate(this.OnEndHasCoverage);
            }
            if ((this.onHasCoverageCompletedDelegate == null)) {
                this.onHasCoverageCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnHasCoverageCompleted);
            }
            base.InvokeAsync(this.onBeginHasCoverageDelegate, new object[] {
                        telephoneNumber}, this.onEndHasCoverageDelegate, this.onHasCoverageCompletedDelegate, userState);
        }
    }
}
