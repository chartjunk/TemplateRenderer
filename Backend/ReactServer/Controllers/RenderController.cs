using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReactServer.Setups;

namespace ReactServer.Controllers
{
    public class RenderController : Controller
    {
        private readonly EntriesSetup entriesSetup;

        public RenderController(EntriesSetup entriesSetup) => this.entriesSetup = entriesSetup;

        public IActionResult Index([FromQuery]string entryKey)
        {
            var result = React.ReactEnvironment.Current.Babel.Transform(@"
const Template = ({ entry }) => (
  <div style={{ width: '100%', padding: '10px' }}>
  
    {/* Top divider */}
    <div style={{ borderWidth: '1px', borderStyle: 'solid', marginBottom: '10px' }} />
  
    {/* Top row */}
    <div style={{
      display: 'flex',
      flexFlow: 'row nowrap',
    }}>      
      {/* Account number boxes */}
      <div style={{
        display: 'flex',
        flex: '1',
      }}>
      
        <div style={{
          display: 'flex',
          flex: '0 0',
          flexFlow: 'row-reverse',
          textAlign: 'right',
          borderStyle: 'none solid solid none',
          padding: '10px 4px 10px 0',
          borderWidth: '3px',
          minWidth: '128px',
        }}>
          Saajan
          <br/>tilinumero
          <br/>Mottagarens
          <br/>kontonummer
        </div>
      
        {/* IBAN-boksi*/}
        <div style={{
          display: 'flex',
          flexFlow: 'column',
          flex: '1 1 0%',
          borderStyle: 'none solid solid none',
          borderWidth: '3px',
        }}>
          <div style={{
            display: 'flex',
            flex: '0',
            paddingLeft: '4px'
          }}>
            IBAN
          </div>
          <div style={{
            display: 'flex',
            flex: '1 0',
            paddingLeft: '10px'
          }}>
            <big><b>{entry.receiver.iban}</b></big>
          </div>
        </div>
        <div style={{
          display: 'flex',
          flexFlow: 'column',
          flex: '0 1 calc(50% - 64px)',
          borderStyle: 'none none solid none',
          borderWidth: '3px',
        }}>

          <div style={{
            display: 'flex',
            flex: '0',
            paddingLeft: '4px'
          }}>
            BIC
          </div>
          <div style={{
            display: 'flex',
            flex: '1 0',
            paddingLeft: '40px'
          }}>
            <big><b>{entry.receiver.bic}</b></big>
          </div>
        </div>
      </div>
    </div>
    
    {/* Middle row */}
    <div style={{
      display: 'flex',
      flexFlow: 'row nowrap',
      flex: '1'
    }}>
    
      {/* Left side */}
      <div style={{
        display: 'flex',
        flexFlow: 'column',
        flex: '1'
      }}>
      
        {/* Left side first row (Receiver) */}
        <div style={{
          display: 'flex',
          flexFlow: 'row nowrap',
          flex: '0'
        }}>
          <div style={{
            display: 'flex',
            flexFlow: 'row-reverse',
            flex: '0 0',
            textAlign: 'right',
            borderStyle: 'none solid solid none',
            padding: '10px 4px 10px 0',
            borderWidth: '3px',
            minWidth: '128px',
          }}>
            Saaja
            <br/>Mottagare
          </div>
          
          {/* Receiver infos */}
          <div style={{
            display: 'flex',
            flex: '1 1',
            borderStyle: 'none solid solid none',
            borderWidth: '3px',
            padding: '3px 10px',
          }}>
            <big>
              <b>
                {entry.receiver.companynameshort} {entry.receiver.companyid}
                <br/>{entry.receiver.companyname}
                <br/>{entry.receiver.postalcode} {entry.receiver.postoffice}
              </b>
            </big>
          </div>
        </div>
        
        {/* Left side second row (Payer) */}
        <div style={{
          display: 'flex',
          flexFlow: 'row nowrap',
          flex: '1'
        }}>
          <div style={{
            display: 'flex',
            flexFlow: 'row-reverse',
            flex: '0 0',
            textAlign: 'right',
            borderStyle: 'none none none none',
            padding: '10px 8px 10px 0',
            borderWidth: '3px',
            minWidth: '128px',
          }}>
            Maksajan
            <br/>nimi ja
            <br/>osoite
            <br/>Betalarens
            <br/>namn
            <br/>och address
          </div>
          
          {/* Payer infos */}
          <div style={{
            display: 'flex',
            flex: '1 1',
            borderStyle: 'none solid none none',
            borderWidth: '3px',
            padding: '30px 15px',
          }}>
            <big>
              <b>
                {entry.payer.name}
                <br/>{entry.payer.address}
                <br/>{entry.payer.postalcode} {entry.payer.postoffice}
              </b>
            </big>
          </div>
        </div>
        
        {/* Left side third row (signature) */}
        <div style={{
          display: 'flex',
          flexFlow: 'row nowrap',
          flex: '0'
        }}>
          <div style={{
            display: 'flex',
            flexFlow: 'row-reverse',
            flex: '0 0',
            textAlign: 'right',
            borderStyle: 'none none solid none',
            borderWidth: '3px',
            padding: '10px 8px 10px 0',
            minWidth: '128px',
          }}>
            Alle-
            <br/>kirjoitus
            <br/>Underskrift
          </div>
          
          {/* Signature line */}
          <div style={{
            display: 'flex',
            flex: '1 1',
            flexFlow: 'column-reverse',
            borderStyle: 'none solid solid none',
            borderWidth: '3px',
            paddingBottom: '14px'
          }}>
            <div style={{
              borderStyle: 'none none solid none',
              borderWidth: '1px',
            }}>
            </div>
          </div>
        </div>
        
      </div>
      
      {/* Right side */}
      <div style={{
        display: 'flex',
        flexFlow: 'column',
        flex: '0 1 calc(50% - 64px)',
      }}>
        <div style={{
          display: 'flex',
          flex: '1',
          borderStyle: 'none none solid none',
          borderWidth: '3px',
          padding: '3px 40px',
        }}>
          <big>
            <b>
              LASKUNUMERO {entry.invoicenumber}
            </b>
          </big>
        </div>
        
        {/* Reference number */}
        <div style={{
          display: 'flex',
          flexFlow: 'row nowrap',
          flex: '0'
        }}>
          <div style={{
            display: 'flex',
            flex: '0 0 65px',
            borderStyle: 'none solid solid none',
            borderWidth: '3px',
            padding: '3px',
          }}>
            Viitenro.
            <br/>Ref.nr
          </div>
          
          {/* Reference number value */}
          <div style={{
            display: 'flex',
            flex: '1 1',
            flexFlow: 'column-reverse',
            borderStyle: 'none none solid none',
            borderWidth: '3px',
            paddingLeft: '28px',
            paddingBottom: '5px'
          }}>
            <big><b>{entry.viitenro}</b></big>
          </div>
        </div>
      </div>
    </div>
    
    {/* Bottom row */}
    <div style={{
      display: 'flex',
      flexFlow: 'row nowrap',
    }}>
      
      <div style={{
        display: 'flex',
        flex: '1',
      }}>
      
        <div style={{
          display: 'flex',
          flex: '0 0',
          flexFlow: 'row-reverse',
          textAlign: 'right',
          borderStyle: 'none solid solid none',
          padding: '4px',
          borderWidth: '3px',
          minWidth: '128px',
        }}>
          Tilitä nro
          <br/>Från konto nr
        </div>
        
        <div style={{
          display: 'flex',
          flexFlow: 'column',
          flex: '1',
          borderStyle: 'none solid solid none',
          borderWidth: '3px',
        }} />
        
        {/* Due date and sum */}
        <div style={{
          display: 'flex',
          flexFlow: 'row nowrap',
          flex: '0 1 calc(50% - 64px)'
        }}>
          <div style={{
            display: 'flex',
            flex: '0 0 65px',
            borderStyle: 'none solid solid none',
            borderWidth: '3px',
            padding: '3px',
          }}>
            Eräpäivä
            <br/>Förf.dag
          </div>
          
          {/* Due date value */}
          <div style={{
            display: 'flex',
            flex: '1 1',
            flexFlow: 'column-reverse',
            borderStyle: 'none none solid none',
            borderWidth: '3px',
            paddingLeft: '28px',
            paddingBottom: '5px'
          }}>
            <big><b>{entry.lastpaymentdate}</b></big>
          </div>
          
          <div style={{
            display: 'flex',
            flex: '0 0 65px',
            borderStyle: 'none none solid solid',
            borderWidth: '3px',
            padding: '3px',
          }}>
            Euro
          </div>
          
          {/* Sum */}
          <div style={{
            display: 'flex',
            flex: '1 1',
            flexFlow: 'column-reverse',
            borderStyle: 'none none solid none',
            borderWidth: '3px',
            paddingLeft: '28px',
            paddingBottom: '5px'
          }}>
            <big><b>{entry.amountineuros}</b></big>
          </div>          
        </div>
      </div>
    </div>
    
  </div>
);"

+ "ReactDOM.render(<Template entry={window.__entry} />, document.getElementById('content'));");

            ViewBag.Template = result;
            ViewBag.Entry = JsonConvert.DeserializeObject(@"{
  ""lastpaymentdate"": ""12.34.2056"",
  ""invoicenumber"": ""1234567"",
  ""receiver"": {
    ""companynameshort"": ""OYOY"",
    ""companyname"": ""Osakeyhtiö OY"",
    ""postalcode"": ""33720"",
    ""postoffice"": ""TAMPERE"",
    ""companyid"": ""1234567-8"",
    ""iban"": ""FI12 3456 7890 1234 56"",
    ""bic"": ""OKOKOKOHH""
  },
  ""payer"": {
    ""name"": ""TESTI ASIAKAS"",
    ""phone"": ""0401234567"",
    ""address"": ""Insinöörinkatu 5"",
    ""postalcode"": ""33720"",
    ""postoffice"": ""Tampere""
  },
  ""viitenro"": ""12 3456 7890 12345"",
  ""amountineuros"": ""100,00""
}");
            return View();
        }
    }
}