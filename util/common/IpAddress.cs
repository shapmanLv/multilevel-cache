﻿using System.Net;

namespace common;
public static class IpAddress
{
    public static async Task<string?> GetIpAsync() 
        => (await Dns.GetHostAddressesAsync(Dns.GetHostName()))
        .FirstOrDefault(_ => _.AddressFamily is System.Net.Sockets.AddressFamily.InterNetwork)
        ?.ToString();
}
