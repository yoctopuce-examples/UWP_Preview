/*********************************************************************
 *
 * $Id: YNetwork.cs 29083 2017-11-03 17:48:44Z seb $
 *
 * Implements FindNetwork(), the high-level API for Network functions
 *
 * - - - - - - - - - License information: - - - - - - - - -
 *
 *  Copyright (C) 2011 and beyond by Yoctopuce Sarl, Switzerland.
 *
 *  Yoctopuce Sarl (hereafter Licensor) grants to you a perpetual
 *  non-exclusive license to use, modify, copy and integrate this
 *  file into your software for the sole purpose of interfacing
 *  with Yoctopuce products.
 *
 *  You may reproduce and distribute copies of this file in
 *  source or object form, as long as the sole purpose of this
 *  code is to interface with Yoctopuce products. You must retain
 *  this notice in the distributed source file.
 *
 *  You should refer to Yoctopuce General Terms and Conditions
 *  for additional information regarding your rights and
 *  obligations.
 *
 *  THE SOFTWARE AND DOCUMENTATION ARE PROVIDED 'AS IS' WITHOUT
 *  WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING
 *  WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY, FITNESS
 *  FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO
 *  EVENT SHALL LICENSOR BE LIABLE FOR ANY INCIDENTAL, SPECIAL,
 *  INDIRECT OR CONSEQUENTIAL DAMAGES, LOST PROFITS OR LOST DATA,
 *  COST OF PROCUREMENT OF SUBSTITUTE GOODS, TECHNOLOGY OR
 *  SERVICES, ANY CLAIMS BY THIRD PARTIES (INCLUDING BUT NOT
 *  LIMITED TO ANY DEFENSE THEREOF), ANY CLAIMS FOR INDEMNITY OR
 *  CONTRIBUTION, OR OTHER SIMILAR COSTS, WHETHER ASSERTED ON THE
 *  BASIS OF CONTRACT, TORT (INCLUDING NEGLIGENCE), BREACH OF
 *  WARRANTY, OR OTHERWISE.
 *
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace com.yoctopuce.YoctoAPI
{

//--- (YNetwork return codes)
//--- (end of YNetwork return codes)
//--- (YNetwork class start)
/**
 * <summary>
 *   YNetwork Class: Network function interface
 * <para>
 *   YNetwork objects provide access to TCP/IP parameters of Yoctopuce
 *   modules that include a built-in network interface.
 * </para>
 * </summary>
 */
public class YNetwork : YFunction
{
//--- (end of YNetwork class start)
//--- (YNetwork definitions)
    /**
     * <summary>
     *   invalid readiness value
     * </summary>
     */
    public const int READINESS_DOWN = 0;
    public const int READINESS_EXISTS = 1;
    public const int READINESS_LINKED = 2;
    public const int READINESS_LAN_OK = 3;
    public const int READINESS_WWW_OK = 4;
    public const int READINESS_INVALID = -1;
    /**
     * <summary>
     *   invalid macAddress value
     * </summary>
     */
    public const  string MACADDRESS_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid ipAddress value
     * </summary>
     */
    public const  string IPADDRESS_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid subnetMask value
     * </summary>
     */
    public const  string SUBNETMASK_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid router value
     * </summary>
     */
    public const  string ROUTER_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid ipConfig value
     * </summary>
     */
    public const  string IPCONFIG_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid primaryDNS value
     * </summary>
     */
    public const  string PRIMARYDNS_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid secondaryDNS value
     * </summary>
     */
    public const  string SECONDARYDNS_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid ntpServer value
     * </summary>
     */
    public const  string NTPSERVER_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid userPassword value
     * </summary>
     */
    public const  string USERPASSWORD_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid adminPassword value
     * </summary>
     */
    public const  string ADMINPASSWORD_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid httpPort value
     * </summary>
     */
    public const  int HTTPPORT_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid defaultPage value
     * </summary>
     */
    public const  string DEFAULTPAGE_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid discoverable value
     * </summary>
     */
    public const int DISCOVERABLE_FALSE = 0;
    public const int DISCOVERABLE_TRUE = 1;
    public const int DISCOVERABLE_INVALID = -1;
    /**
     * <summary>
     *   invalid wwwWatchdogDelay value
     * </summary>
     */
    public const  int WWWWATCHDOGDELAY_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid callbackUrl value
     * </summary>
     */
    public const  string CALLBACKURL_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid callbackMethod value
     * </summary>
     */
    public const int CALLBACKMETHOD_POST = 0;
    public const int CALLBACKMETHOD_GET = 1;
    public const int CALLBACKMETHOD_PUT = 2;
    public const int CALLBACKMETHOD_INVALID = -1;
    /**
     * <summary>
     *   invalid callbackEncoding value
     * </summary>
     */
    public const int CALLBACKENCODING_FORM = 0;
    public const int CALLBACKENCODING_JSON = 1;
    public const int CALLBACKENCODING_JSON_ARRAY = 2;
    public const int CALLBACKENCODING_CSV = 3;
    public const int CALLBACKENCODING_YOCTO_API = 4;
    public const int CALLBACKENCODING_JSON_NUM = 5;
    public const int CALLBACKENCODING_EMONCMS = 6;
    public const int CALLBACKENCODING_AZURE = 7;
    public const int CALLBACKENCODING_INFLUXDB = 8;
    public const int CALLBACKENCODING_MQTT = 9;
    public const int CALLBACKENCODING_YOCTO_API_JZON = 10;
    public const int CALLBACKENCODING_INVALID = -1;
    /**
     * <summary>
     *   invalid callbackCredentials value
     * </summary>
     */
    public const  string CALLBACKCREDENTIALS_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid callbackInitialDelay value
     * </summary>
     */
    public const  int CALLBACKINITIALDELAY_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid callbackSchedule value
     * </summary>
     */
    public const  string CALLBACKSCHEDULE_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid callbackMinDelay value
     * </summary>
     */
    public const  int CALLBACKMINDELAY_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid callbackMaxDelay value
     * </summary>
     */
    public const  int CALLBACKMAXDELAY_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid poeCurrent value
     * </summary>
     */
    public const  int POECURRENT_INVALID = YAPI.INVALID_UINT;
    protected int _readiness = READINESS_INVALID;
    protected string _macAddress = MACADDRESS_INVALID;
    protected string _ipAddress = IPADDRESS_INVALID;
    protected string _subnetMask = SUBNETMASK_INVALID;
    protected string _router = ROUTER_INVALID;
    protected string _ipConfig = IPCONFIG_INVALID;
    protected string _primaryDNS = PRIMARYDNS_INVALID;
    protected string _secondaryDNS = SECONDARYDNS_INVALID;
    protected string _ntpServer = NTPSERVER_INVALID;
    protected string _userPassword = USERPASSWORD_INVALID;
    protected string _adminPassword = ADMINPASSWORD_INVALID;
    protected int _httpPort = HTTPPORT_INVALID;
    protected string _defaultPage = DEFAULTPAGE_INVALID;
    protected int _discoverable = DISCOVERABLE_INVALID;
    protected int _wwwWatchdogDelay = WWWWATCHDOGDELAY_INVALID;
    protected string _callbackUrl = CALLBACKURL_INVALID;
    protected int _callbackMethod = CALLBACKMETHOD_INVALID;
    protected int _callbackEncoding = CALLBACKENCODING_INVALID;
    protected string _callbackCredentials = CALLBACKCREDENTIALS_INVALID;
    protected int _callbackInitialDelay = CALLBACKINITIALDELAY_INVALID;
    protected string _callbackSchedule = CALLBACKSCHEDULE_INVALID;
    protected int _callbackMinDelay = CALLBACKMINDELAY_INVALID;
    protected int _callbackMaxDelay = CALLBACKMAXDELAY_INVALID;
    protected int _poeCurrent = POECURRENT_INVALID;
    protected ValueCallback _valueCallbackNetwork = null;

    public new delegate Task ValueCallback(YNetwork func, string value);
    public new delegate Task TimedReportCallback(YNetwork func, YMeasure measure);
    //--- (end of YNetwork definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YNetwork(YAPIContext ctx, string func)
        : base(ctx, func, "Network")
    {
        //--- (YNetwork attributes initialization)
        //--- (end of YNetwork attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YNetwork(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YNetwork implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("readiness")) {
            _readiness = json_val.getInt("readiness");
        }
        if (json_val.has("macAddress")) {
            _macAddress = json_val.getString("macAddress");
        }
        if (json_val.has("ipAddress")) {
            _ipAddress = json_val.getString("ipAddress");
        }
        if (json_val.has("subnetMask")) {
            _subnetMask = json_val.getString("subnetMask");
        }
        if (json_val.has("router")) {
            _router = json_val.getString("router");
        }
        if (json_val.has("ipConfig")) {
            _ipConfig = json_val.getString("ipConfig");
        }
        if (json_val.has("primaryDNS")) {
            _primaryDNS = json_val.getString("primaryDNS");
        }
        if (json_val.has("secondaryDNS")) {
            _secondaryDNS = json_val.getString("secondaryDNS");
        }
        if (json_val.has("ntpServer")) {
            _ntpServer = json_val.getString("ntpServer");
        }
        if (json_val.has("userPassword")) {
            _userPassword = json_val.getString("userPassword");
        }
        if (json_val.has("adminPassword")) {
            _adminPassword = json_val.getString("adminPassword");
        }
        if (json_val.has("httpPort")) {
            _httpPort = json_val.getInt("httpPort");
        }
        if (json_val.has("defaultPage")) {
            _defaultPage = json_val.getString("defaultPage");
        }
        if (json_val.has("discoverable")) {
            _discoverable = json_val.getInt("discoverable") > 0 ? 1 : 0;
        }
        if (json_val.has("wwwWatchdogDelay")) {
            _wwwWatchdogDelay = json_val.getInt("wwwWatchdogDelay");
        }
        if (json_val.has("callbackUrl")) {
            _callbackUrl = json_val.getString("callbackUrl");
        }
        if (json_val.has("callbackMethod")) {
            _callbackMethod = json_val.getInt("callbackMethod");
        }
        if (json_val.has("callbackEncoding")) {
            _callbackEncoding = json_val.getInt("callbackEncoding");
        }
        if (json_val.has("callbackCredentials")) {
            _callbackCredentials = json_val.getString("callbackCredentials");
        }
        if (json_val.has("callbackInitialDelay")) {
            _callbackInitialDelay = json_val.getInt("callbackInitialDelay");
        }
        if (json_val.has("callbackSchedule")) {
            _callbackSchedule = json_val.getString("callbackSchedule");
        }
        if (json_val.has("callbackMinDelay")) {
            _callbackMinDelay = json_val.getInt("callbackMinDelay");
        }
        if (json_val.has("callbackMaxDelay")) {
            _callbackMaxDelay = json_val.getInt("callbackMaxDelay");
        }
        if (json_val.has("poeCurrent")) {
            _poeCurrent = json_val.getInt("poeCurrent");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the current established working mode of the network interface.
     * <para>
     *   Level zero (DOWN_0) means that no hardware link has been detected. Either there is no signal
     *   on the network cable, or the selected wireless access point cannot be detected.
     *   Level 1 (LIVE_1) is reached when the network is detected, but is not yet connected.
     *   For a wireless network, this shows that the requested SSID is present.
     *   Level 2 (LINK_2) is reached when the hardware connection is established.
     *   For a wired network connection, level 2 means that the cable is attached at both ends.
     *   For a connection to a wireless access point, it shows that the security parameters
     *   are properly configured. For an ad-hoc wireless connection, it means that there is
     *   at least one other device connected on the ad-hoc network.
     *   Level 3 (DHCP_3) is reached when an IP address has been obtained using DHCP.
     *   Level 4 (DNS_4) is reached when the DNS server is reachable on the network.
     *   Level 5 (WWW_5) is reached when global connectivity is demonstrated by properly loading the
     *   current time from an NTP server.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YNetwork.READINESS_DOWN</c>, <c>YNetwork.READINESS_EXISTS</c>,
     *   <c>YNetwork.READINESS_LINKED</c>, <c>YNetwork.READINESS_LAN_OK</c> and
     *   <c>YNetwork.READINESS_WWW_OK</c> corresponding to the current established working mode of the network interface
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.READINESS_INVALID</c>.
     * </para>
     */
    public async Task<int> get_readiness()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return READINESS_INVALID;
            }
        }
        res = _readiness;
        return res;
    }


    /**
     * <summary>
     *   Returns the MAC address of the network interface.
     * <para>
     *   The MAC address is also available on a sticker
     *   on the module, in both numeric and barcode forms.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the MAC address of the network interface
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.MACADDRESS_INVALID</c>.
     * </para>
     */
    public async Task<string> get_macAddress()
    {
        string res;
        if (_cacheExpiration == 0) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MACADDRESS_INVALID;
            }
        }
        res = _macAddress;
        return res;
    }


    /**
     * <summary>
     *   Returns the IP address currently in use by the device.
     * <para>
     *   The address may have been configured
     *   statically, or provided by a DHCP server.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the IP address currently in use by the device
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.IPADDRESS_INVALID</c>.
     * </para>
     */
    public async Task<string> get_ipAddress()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return IPADDRESS_INVALID;
            }
        }
        res = _ipAddress;
        return res;
    }


    /**
     * <summary>
     *   Returns the subnet mask currently used by the device.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the subnet mask currently used by the device
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.SUBNETMASK_INVALID</c>.
     * </para>
     */
    public async Task<string> get_subnetMask()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SUBNETMASK_INVALID;
            }
        }
        res = _subnetMask;
        return res;
    }


    /**
     * <summary>
     *   Returns the IP address of the router on the device subnet (default gateway).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the IP address of the router on the device subnet (default gateway)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.ROUTER_INVALID</c>.
     * </para>
     */
    public async Task<string> get_router()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ROUTER_INVALID;
            }
        }
        res = _router;
        return res;
    }


    /**
     * <summary>
     *   Returns the IP configuration of the network interface.
     * <para>
     * </para>
     * <para>
     *   If the network interface is setup to use a static IP address, the string starts with "STATIC:" and
     *   is followed by three
     *   parameters, separated by "/". The first is the device IP address, followed by the subnet mask
     *   length, and finally the
     *   router IP address (default gateway). For instance: "STATIC:192.168.1.14/16/192.168.1.1"
     * </para>
     * <para>
     *   If the network interface is configured to receive its IP from a DHCP server, the string start with
     *   "DHCP:" and is followed by
     *   three parameters separated by "/". The first is the fallback IP address, then the fallback subnet
     *   mask length and finally the
     *   fallback router IP address. These three parameters are used when no DHCP reply is received.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the IP configuration of the network interface
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.IPCONFIG_INVALID</c>.
     * </para>
     */
    public async Task<string> get_ipConfig()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return IPCONFIG_INVALID;
            }
        }
        res = _ipConfig;
        return res;
    }


    public async Task<int> set_ipConfig(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("ipConfig",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the IP address of the primary name server to be used by the module.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the IP address of the primary name server to be used by the module
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.PRIMARYDNS_INVALID</c>.
     * </para>
     */
    public async Task<string> get_primaryDNS()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PRIMARYDNS_INVALID;
            }
        }
        res = _primaryDNS;
        return res;
    }


    /**
     * <summary>
     *   Changes the IP address of the primary name server to be used by the module.
     * <para>
     *   When using DHCP, if a value is specified, it overrides the value received from the DHCP server.
     *   Remember to call the <c>saveToFlash()</c> method and then to reboot the module to apply this setting.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the IP address of the primary name server to be used by the module
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_primaryDNS(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("primaryDNS",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the IP address of the secondary name server to be used by the module.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the IP address of the secondary name server to be used by the module
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.SECONDARYDNS_INVALID</c>.
     * </para>
     */
    public async Task<string> get_secondaryDNS()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SECONDARYDNS_INVALID;
            }
        }
        res = _secondaryDNS;
        return res;
    }


    /**
     * <summary>
     *   Changes the IP address of the secondary name server to be used by the module.
     * <para>
     *   When using DHCP, if a value is specified, it overrides the value received from the DHCP server.
     *   Remember to call the <c>saveToFlash()</c> method and then to reboot the module to apply this setting.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the IP address of the secondary name server to be used by the module
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_secondaryDNS(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("secondaryDNS",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the IP address of the NTP server to be used by the device.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the IP address of the NTP server to be used by the device
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.NTPSERVER_INVALID</c>.
     * </para>
     */
    public async Task<string> get_ntpServer()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return NTPSERVER_INVALID;
            }
        }
        res = _ntpServer;
        return res;
    }


    /**
     * <summary>
     *   Changes the IP address of the NTP server to be used by the module.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method and then to reboot the module to apply this setting.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the IP address of the NTP server to be used by the module
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_ntpServer(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("ntpServer",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns a hash string if a password has been set for "user" user,
     *   or an empty string otherwise.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to a hash string if a password has been set for "user" user,
     *   or an empty string otherwise
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.USERPASSWORD_INVALID</c>.
     * </para>
     */
    public async Task<string> get_userPassword()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return USERPASSWORD_INVALID;
            }
        }
        res = _userPassword;
        return res;
    }


    /**
     * <summary>
     *   Changes the password for the "user" user.
     * <para>
     *   This password becomes instantly required
     *   to perform any use of the module. If the specified value is an
     *   empty string, a password is not required anymore.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the password for the "user" user
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_userPassword(string  newval)
    {
        string rest_val;
        if (newval.Length > YAPI.HASH_BUF_SIZE)
            _throw(YAPI.INVALID_ARGUMENT,"Password too long :" + newval);
        rest_val = newval;
        await _setAttr("userPassword",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns a hash string if a password has been set for user "admin",
     *   or an empty string otherwise.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to a hash string if a password has been set for user "admin",
     *   or an empty string otherwise
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.ADMINPASSWORD_INVALID</c>.
     * </para>
     */
    public async Task<string> get_adminPassword()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ADMINPASSWORD_INVALID;
            }
        }
        res = _adminPassword;
        return res;
    }


    /**
     * <summary>
     *   Changes the password for the "admin" user.
     * <para>
     *   This password becomes instantly required
     *   to perform any change of the module state. If the specified value is an
     *   empty string, a password is not required anymore.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the password for the "admin" user
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_adminPassword(string  newval)
    {
        string rest_val;
        if (newval.Length > YAPI.HASH_BUF_SIZE)
            _throw(YAPI.INVALID_ARGUMENT,"Password too long :" + newval);
        rest_val = newval;
        await _setAttr("adminPassword",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the HTML page to serve for the URL "/"" of the hub.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the HTML page to serve for the URL "/"" of the hub
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.HTTPPORT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_httpPort()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return HTTPPORT_INVALID;
            }
        }
        res = _httpPort;
        return res;
    }


    /**
     * <summary>
     *   Changes the default HTML page returned by the hub.
     * <para>
     *   If not value are set the hub return
     *   "index.html" which is the web interface of the hub. It is possible de change this page
     *   for file that has been uploaded on the hub.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the default HTML page returned by the hub
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_httpPort(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("httpPort",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the HTML page to serve for the URL "/"" of the hub.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the HTML page to serve for the URL "/"" of the hub
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.DEFAULTPAGE_INVALID</c>.
     * </para>
     */
    public async Task<string> get_defaultPage()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return DEFAULTPAGE_INVALID;
            }
        }
        res = _defaultPage;
        return res;
    }


    /**
     * <summary>
     *   Changes the default HTML page returned by the hub.
     * <para>
     *   If not value are set the hub return
     *   "index.html" which is the web interface of the hub. It is possible de change this page
     *   for file that has been uploaded on the hub.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the default HTML page returned by the hub
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_defaultPage(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("defaultPage",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the activation state of the multicast announce protocols to allow easy
     *   discovery of the module in the network neighborhood (uPnP/Bonjour protocol).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YNetwork.DISCOVERABLE_FALSE</c> or <c>YNetwork.DISCOVERABLE_TRUE</c>, according to the
     *   activation state of the multicast announce protocols to allow easy
     *   discovery of the module in the network neighborhood (uPnP/Bonjour protocol)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.DISCOVERABLE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_discoverable()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return DISCOVERABLE_INVALID;
            }
        }
        res = _discoverable;
        return res;
    }


    /**
     * <summary>
     *   Changes the activation state of the multicast announce protocols to allow easy
     *   discovery of the module in the network neighborhood (uPnP/Bonjour protocol).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YNetwork.DISCOVERABLE_FALSE</c> or <c>YNetwork.DISCOVERABLE_TRUE</c>, according to the
     *   activation state of the multicast announce protocols to allow easy
     *   discovery of the module in the network neighborhood (uPnP/Bonjour protocol)
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_discoverable(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("discoverable",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the allowed downtime of the WWW link (in seconds) before triggering an automated
     *   reboot to try to recover Internet connectivity.
     * <para>
     *   A zero value disables automated reboot
     *   in case of Internet connectivity loss.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the allowed downtime of the WWW link (in seconds) before triggering an automated
     *   reboot to try to recover Internet connectivity
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.WWWWATCHDOGDELAY_INVALID</c>.
     * </para>
     */
    public async Task<int> get_wwwWatchdogDelay()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return WWWWATCHDOGDELAY_INVALID;
            }
        }
        res = _wwwWatchdogDelay;
        return res;
    }


    /**
     * <summary>
     *   Changes the allowed downtime of the WWW link (in seconds) before triggering an automated
     *   reboot to try to recover Internet connectivity.
     * <para>
     *   A zero value disables automated reboot
     *   in case of Internet connectivity loss. The smallest valid non-zero timeout is
     *   90 seconds.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the allowed downtime of the WWW link (in seconds) before triggering an automated
     *   reboot to try to recover Internet connectivity
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_wwwWatchdogDelay(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("wwwWatchdogDelay",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the callback URL to notify of significant state changes.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the callback URL to notify of significant state changes
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.CALLBACKURL_INVALID</c>.
     * </para>
     */
    public async Task<string> get_callbackUrl()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CALLBACKURL_INVALID;
            }
        }
        res = _callbackUrl;
        return res;
    }


    /**
     * <summary>
     *   Changes the callback URL to notify significant state changes.
     * <para>
     *   Remember to call the
     *   <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the callback URL to notify significant state changes
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_callbackUrl(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("callbackUrl",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the HTTP method used to notify callbacks for significant state changes.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YNetwork.CALLBACKMETHOD_POST</c>, <c>YNetwork.CALLBACKMETHOD_GET</c> and
     *   <c>YNetwork.CALLBACKMETHOD_PUT</c> corresponding to the HTTP method used to notify callbacks for
     *   significant state changes
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.CALLBACKMETHOD_INVALID</c>.
     * </para>
     */
    public async Task<int> get_callbackMethod()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CALLBACKMETHOD_INVALID;
            }
        }
        res = _callbackMethod;
        return res;
    }


    /**
     * <summary>
     *   Changes the HTTP method used to notify callbacks for significant state changes.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YNetwork.CALLBACKMETHOD_POST</c>, <c>YNetwork.CALLBACKMETHOD_GET</c> and
     *   <c>YNetwork.CALLBACKMETHOD_PUT</c> corresponding to the HTTP method used to notify callbacks for
     *   significant state changes
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_callbackMethod(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("callbackMethod",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the encoding standard to use for representing notification values.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YNetwork.CALLBACKENCODING_FORM</c>, <c>YNetwork.CALLBACKENCODING_JSON</c>,
     *   <c>YNetwork.CALLBACKENCODING_JSON_ARRAY</c>, <c>YNetwork.CALLBACKENCODING_CSV</c>,
     *   <c>YNetwork.CALLBACKENCODING_YOCTO_API</c>, <c>YNetwork.CALLBACKENCODING_JSON_NUM</c>,
     *   <c>YNetwork.CALLBACKENCODING_EMONCMS</c>, <c>YNetwork.CALLBACKENCODING_AZURE</c>,
     *   <c>YNetwork.CALLBACKENCODING_INFLUXDB</c>, <c>YNetwork.CALLBACKENCODING_MQTT</c> and
     *   <c>YNetwork.CALLBACKENCODING_YOCTO_API_JZON</c> corresponding to the encoding standard to use for
     *   representing notification values
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.CALLBACKENCODING_INVALID</c>.
     * </para>
     */
    public async Task<int> get_callbackEncoding()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CALLBACKENCODING_INVALID;
            }
        }
        res = _callbackEncoding;
        return res;
    }


    /**
     * <summary>
     *   Changes the encoding standard to use for representing notification values.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YNetwork.CALLBACKENCODING_FORM</c>, <c>YNetwork.CALLBACKENCODING_JSON</c>,
     *   <c>YNetwork.CALLBACKENCODING_JSON_ARRAY</c>, <c>YNetwork.CALLBACKENCODING_CSV</c>,
     *   <c>YNetwork.CALLBACKENCODING_YOCTO_API</c>, <c>YNetwork.CALLBACKENCODING_JSON_NUM</c>,
     *   <c>YNetwork.CALLBACKENCODING_EMONCMS</c>, <c>YNetwork.CALLBACKENCODING_AZURE</c>,
     *   <c>YNetwork.CALLBACKENCODING_INFLUXDB</c>, <c>YNetwork.CALLBACKENCODING_MQTT</c> and
     *   <c>YNetwork.CALLBACKENCODING_YOCTO_API_JZON</c> corresponding to the encoding standard to use for
     *   representing notification values
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_callbackEncoding(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("callbackEncoding",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns a hashed version of the notification callback credentials if set,
     *   or an empty string otherwise.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to a hashed version of the notification callback credentials if set,
     *   or an empty string otherwise
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.CALLBACKCREDENTIALS_INVALID</c>.
     * </para>
     */
    public async Task<string> get_callbackCredentials()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CALLBACKCREDENTIALS_INVALID;
            }
        }
        res = _callbackCredentials;
        return res;
    }


    /**
     * <summary>
     *   Changes the credentials required to connect to the callback address.
     * <para>
     *   The credentials
     *   must be provided as returned by function <c>get_callbackCredentials</c>,
     *   in the form <c>username:hash</c>. The method used to compute the hash varies according
     *   to the the authentication scheme implemented by the callback, For Basic authentication,
     *   the hash is the MD5 of the string <c>username:password</c>. For Digest authentication,
     *   the hash is the MD5 of the string <c>username:realm:password</c>. For a simpler
     *   way to configure callback credentials, use function <c>callbackLogin</c> instead.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the credentials required to connect to the callback address
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_callbackCredentials(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("callbackCredentials",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the initial waiting time before first callback notifications, in seconds.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the initial waiting time before first callback notifications, in seconds
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.CALLBACKINITIALDELAY_INVALID</c>.
     * </para>
     */
    public async Task<int> get_callbackInitialDelay()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CALLBACKINITIALDELAY_INVALID;
            }
        }
        res = _callbackInitialDelay;
        return res;
    }


    /**
     * <summary>
     *   Changes the initial waiting time before first callback notifications, in seconds.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the initial waiting time before first callback notifications, in seconds
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_callbackInitialDelay(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("callbackInitialDelay",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the HTTP callback schedule strategy, as a text string.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the HTTP callback schedule strategy, as a text string
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.CALLBACKSCHEDULE_INVALID</c>.
     * </para>
     */
    public async Task<string> get_callbackSchedule()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CALLBACKSCHEDULE_INVALID;
            }
        }
        res = _callbackSchedule;
        return res;
    }


    /**
     * <summary>
     *   Changes the HTTP callback schedule strategy, as a text string.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the HTTP callback schedule strategy, as a text string
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_callbackSchedule(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("callbackSchedule",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the minimum waiting time between two HTTP callbacks, in seconds.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the minimum waiting time between two HTTP callbacks, in seconds
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.CALLBACKMINDELAY_INVALID</c>.
     * </para>
     */
    public async Task<int> get_callbackMinDelay()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CALLBACKMINDELAY_INVALID;
            }
        }
        res = _callbackMinDelay;
        return res;
    }


    /**
     * <summary>
     *   Changes the minimum waiting time between two HTTP callbacks, in seconds.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the minimum waiting time between two HTTP callbacks, in seconds
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_callbackMinDelay(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("callbackMinDelay",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the waiting time between two HTTP callbacks when there is nothing new.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the waiting time between two HTTP callbacks when there is nothing new
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.CALLBACKMAXDELAY_INVALID</c>.
     * </para>
     */
    public async Task<int> get_callbackMaxDelay()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return CALLBACKMAXDELAY_INVALID;
            }
        }
        res = _callbackMaxDelay;
        return res;
    }


    /**
     * <summary>
     *   Changes the waiting time between two HTTP callbacks when there is nothing new.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the waiting time between two HTTP callbacks when there is nothing new
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_callbackMaxDelay(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("callbackMaxDelay",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current consumed by the module from Power-over-Ethernet (PoE), in milli-amps.
     * <para>
     *   The current consumption is measured after converting PoE source to 5 Volt, and should
     *   never exceed 1800 mA.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current consumed by the module from Power-over-Ethernet (PoE), in milli-amps
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YNetwork.POECURRENT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_poeCurrent()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return POECURRENT_INVALID;
            }
        }
        res = _poeCurrent;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a network interface for a given identifier.
     * <para>
     *   The identifier can be specified using several formats:
     * </para>
     * <para>
     * </para>
     * <para>
     *   - FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionLogicalName
     * </para>
     * <para>
     * </para>
     * <para>
     *   This function does not require that the network interface is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YNetwork.isOnline()</c> to test if the network interface is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a network interface by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * <para>
     *   If a call to this object's is_online() method returns FALSE although
     *   you are certain that the matching device is plugged, make sure that you did
     *   call registerHub() at application initialization time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the network interface
     * </param>
     * <returns>
     *   a <c>YNetwork</c> object allowing you to drive the network interface.
     * </returns>
     */
    public static YNetwork FindNetwork(string func)
    {
        YNetwork obj;
        obj = (YNetwork) YFunction._FindFromCache("Network", func);
        if (obj == null) {
            obj = new YNetwork(func);
            YFunction._AddToCache("Network",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a network interface for a given identifier in a YAPI context.
     * <para>
     *   The identifier can be specified using several formats:
     * </para>
     * <para>
     * </para>
     * <para>
     *   - FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionLogicalName
     * </para>
     * <para>
     * </para>
     * <para>
     *   This function does not require that the network interface is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YNetwork.isOnline()</c> to test if the network interface is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a network interface by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the network interface
     * </param>
     * <returns>
     *   a <c>YNetwork</c> object allowing you to drive the network interface.
     * </returns>
     */
    public static YNetwork FindNetworkInContext(YAPIContext yctx,string func)
    {
        YNetwork obj;
        obj = (YNetwork) YFunction._FindFromCacheInContext(yctx,  "Network", func);
        if (obj == null) {
            obj = new YNetwork(yctx, func);
            YFunction._AddToCache("Network",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Registers the callback function that is invoked on every change of advertised value.
     * <para>
     *   The callback is invoked only during the execution of <c>ySleep</c> or <c>yHandleEvents</c>.
     *   This provides control over the time when the callback is triggered. For good responsiveness, remember to call
     *   one of these two functions periodically. To unregister a callback, pass a null pointer as argument.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="callback">
     *   the callback function to call, or a null pointer. The callback function should take two
     *   arguments: the function object of which the value has changed, and the character string describing
     *   the new advertised value.
     * @noreturn
     * </param>
     */
    public async Task<int> registerValueCallback(ValueCallback callback)
    {
        string val;
        if (callback != null) {
            await YFunction._UpdateValueCallbackList(this, true);
        } else {
            await YFunction._UpdateValueCallbackList(this, false);
        }
        _valueCallbackNetwork = callback;
        // Immediately invoke value callback with current value
        if (callback != null && await this.isOnline()) {
            val = _advertisedValue;
            if (!(val == "")) {
                await this._invokeValueCallback(val);
            }
        }
        return 0;
    }

    public override async Task<int> _invokeValueCallback(string value)
    {
        if (_valueCallbackNetwork != null) {
            await _valueCallbackNetwork(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Changes the configuration of the network interface to enable the use of an
     *   IP address received from a DHCP server.
     * <para>
     *   Until an address is received from a DHCP
     *   server, the module uses the IP parameters specified to this function.
     *   Remember to call the <c>saveToFlash()</c> method and then to reboot the module to apply this setting.
     * </para>
     * </summary>
     * <param name="fallbackIpAddr">
     *   fallback IP address, to be used when no DHCP reply is received
     * </param>
     * <param name="fallbackSubnetMaskLen">
     *   fallback subnet mask length when no DHCP reply is received, as an
     *   integer (eg. 24 means 255.255.255.0)
     * </param>
     * <param name="fallbackRouter">
     *   fallback router IP address, to be used when no DHCP reply is received
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> useDHCP(string fallbackIpAddr,int fallbackSubnetMaskLen,string fallbackRouter)
    {
        return await this.set_ipConfig("DHCP:"+ fallbackIpAddr+"/"+Convert.ToString( fallbackSubnetMaskLen)+"/"+fallbackRouter);
    }

    /**
     * <summary>
     *   Changes the configuration of the network interface to enable the use of an
     *   IP address received from a DHCP server.
     * <para>
     *   Until an address is received from a DHCP
     *   server, the module uses an IP of the network 169.254.0.0/16 (APIPA).
     *   Remember to call the <c>saveToFlash()</c> method and then to reboot the module to apply this setting.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> useDHCPauto()
    {
        return await this.set_ipConfig("DHCP:");
    }

    /**
     * <summary>
     *   Changes the configuration of the network interface to use a static IP address.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method and then to reboot the module to apply this setting.
     * </para>
     * </summary>
     * <param name="ipAddress">
     *   device IP address
     * </param>
     * <param name="subnetMaskLen">
     *   subnet mask length, as an integer (eg. 24 means 255.255.255.0)
     * </param>
     * <param name="router">
     *   router IP address (default gateway)
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> useStaticIP(string ipAddress,int subnetMaskLen,string router)
    {
        return await this.set_ipConfig("STATIC:"+ ipAddress+"/"+Convert.ToString( subnetMaskLen)+"/"+router);
    }

    /**
     * <summary>
     *   Pings host to test the network connectivity.
     * <para>
     *   Sends four ICMP ECHO_REQUEST requests from the
     *   module to the target host. This method returns a string with the result of the
     *   4 ICMP ECHO_REQUEST requests.
     * </para>
     * </summary>
     * <param name="host">
     *   the hostname or the IP address of the target
     * </param>
     * <para>
     * </para>
     * <returns>
     *   a string with the result of the ping.
     * </returns>
     */
    public virtual async Task<string> ping(string host)
    {
        byte[] content;

        content = await this._download("ping.txt?host="+host);
        return YAPI.DefaultEncoding.GetString(content);
    }

    /**
     * <summary>
     *   Trigger an HTTP callback quickly.
     * <para>
     *   This function can even be called within
     *   an HTTP callback, in which case the next callback will be triggered 5 seconds
     *   after the end of the current callback, regardless if the minimum time between
     *   callbacks configured in the device.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> triggerCallback()
    {
        return await this.set_callbackMethod(await this.get_callbackMethod());
    }

    /**
     * <summary>
     *   Setup periodic HTTP callbacks (simplifed function).
     * <para>
     * </para>
     * </summary>
     * <param name="interval">
     *   a string representing the callback periodicity, expressed in
     *   seconds, minutes or hours, eg. "60s", "5m", "1h", "48h".
     * </param>
     * <param name="offset">
     *   an integer representing the time offset relative to the period
     *   when the callback should occur. For instance, if the periodicity is
     *   24h, an offset of 7 will make the callback occur each day at 7AM.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_periodicCallbackSchedule(string interval,int offset)
    {
        return await this.set_callbackSchedule("every "+interval+"+"+Convert.ToString(offset));
    }

    /**
     * <summary>
     *   Continues the enumeration of network interfaces started using <c>yFirstNetwork()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YNetwork</c> object, corresponding to
     *   a network interface currently online, or a <c>null</c> pointer
     *   if there are no more network interfaces to enumerate.
     * </returns>
     */
    public YNetwork nextNetwork()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindNetworkInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of network interfaces currently accessible.
     * <para>
     *   Use the method <c>YNetwork.nextNetwork()</c> to iterate on
     *   next network interfaces.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YNetwork</c> object, corresponding to
     *   the first network interface currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YNetwork FirstNetwork()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Network");
        if (next_hwid == null)  return null;
        return FindNetworkInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of network interfaces currently accessible.
     * <para>
     *   Use the method <c>YNetwork.nextNetwork()</c> to iterate on
     *   next network interfaces.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YNetwork</c> object, corresponding to
     *   the first network interface currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YNetwork FirstNetworkInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Network");
        if (next_hwid == null)  return null;
        return FindNetworkInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YNetwork implementation)
}
}

