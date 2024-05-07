using System.Text;
using GoWasmWrapper;
using WebAssembly;

namespace Pcrd;

public class PcrdWrapper
{
    private readonly string wasmPath;
    public PcrdWrapper(string wasmPath)
    {
        this.wasmPath = wasmPath;
    }
    
    public string CreateSign(string rawJson, string nonce)
    {
        if (_wrapper == null)
        {
            ReloadWasm();
        }
        return _wrapper.RunEvent(1, new Object[] {
            rawJson,
            nonce,
            calcHash(nonce)
        }) as string ?? "";
    }

    private GoWrapper? _wrapper;
    public void ReloadWasm()
    {
        _wrapper = new GoWrapper(Module.ReadFromBinary(wasmPath))
        {
            Global =
            {
                ["myhash"] = new Func<string, double>(myHash),
                ["location"] = new Dictionary<string, object>
                {
                    ["host"] = "pcrdfans.com",
                    ["hostname"] = "pcrdfans.com",
                }
            }
        };
    }

    public string GenNonce()
    {
        const string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
        var rand = new Random();

        return new string(Enumerable.Range(0, 16).Select(_ => chars[rand.Next(36)]).ToArray());
    }

    private double calcHash(string str)
    {
        var text = Encoding.ASCII.GetBytes(str);
        uint _0x473e93, _0x5d587e;
        for (_0x473e93 = 0x1bf52, _0x5d587e = (uint)text.Length; _0x5d587e != 0;)
            _0x473e93 = 0x309 * _0x473e93 ^ text[--_0x5d587e];
        return _0x473e93 >> 0x3;
    }

    private double myHash(string str)
    {
        var text = Encoding.ASCII.GetBytes(str);
        uint _0x473e93, _0x5d587e;
        for (_0x473e93 = 0x202, _0x5d587e = (uint)text.Length; _0x5d587e != 0;)
            _0x473e93 = 0x72 * _0x473e93 ^ text[--_0x5d587e];
        return _0x473e93 >> 0x3;
    }
}
