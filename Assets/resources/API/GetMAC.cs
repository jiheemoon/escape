using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.NetworkInformation;

namespace NetworkInformation
{
    public class GetMac
    {
        string m_strMacAddress;
        NetworkInterface[] m_adapters;

        public GetMac()
        {
            m_strMacAddress = string.Empty;
            m_adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in m_adapters)
            {
                PhysicalAddress pa = adapter.GetPhysicalAddress();
                if ((pa != null) && (!pa.ToString().Equals("")))
                {
                    m_strMacAddress = pa.ToString();
                    break;
                }
            }
        }

        public string MacAddress
        {
            get { return m_strMacAddress; }
        }
    }
}