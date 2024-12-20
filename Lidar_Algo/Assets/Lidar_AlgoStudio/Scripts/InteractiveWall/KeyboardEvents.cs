// Tayde M. Cruz (tayde@algostudio.mx)
// ALGO STUDIO
// 11-08-2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Threading;

public class KeyboardEvents : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan,
        uint dwFlags, uint dwExtraInfo);

    protected Dictionary<char, int> virtualKeyCodes = new Dictionary<char, int>
    {
        {' ', 0x20},
        {'0', 0x30},
        {'1', 0x31},
        {'2', 0x32},
        {'3', 0x33},
        {'4', 0x34},
        {'5', 0x35},
        {'6', 0x36},
        {'7', 0x37},
        {'8', 0x38},
        {'9', 0x39},
        {'A', 0x41},
        {'B', 0x42},
        {'C', 0x43},
        {'D', 0x44},
        {'E', 0x45},
        {'F', 0x46},
        {'G', 0x47},
        {'H', 0x48},
        {'I', 0x49},
        {'J', 0x4A},
        {'K', 0x4B},
        {'L', 0x4C},
        {'M', 0x4D},
        {'N', 0x4E},
        {'O', 0x4F},
        {'P', 0x50},
        {'Q', 0x51},
        {'R', 0x52},
        {'S', 0x53},
        {'T', 0x54},
        {'U', 0x55},
        {'V', 0x56},
        {'W', 0x57},
        {'X', 0x58},
        {'Y', 0x59},
        {'Z', 0x5A}/*,
        {'a', 0x41},
        {'b', 0x42},
        {'c', 0x43},
        {'d', 0x44},
        {'e', 0x45},
        {'f', 0x46},
        {'g', 0x47},
        {'h', 0x48},
        {'i', 0x49},
        {'j', 0x4A},
        {'k', 0x4B},
        {'l', 0x4C},
        {'m', 0x4D},
        {'n', 0x4E},
        {'o', 0x4F},
        {'p', 0x50},
        {'q', 0x51},
        {'r', 0x52},
        {'s', 0x53},
        {'t', 0x54},
        {'u', 0x55},
        {'v', 0x56},
        {'w', 0x57},
        {'x', 0x58},
        {'y', 0x59},
        {'z', 0x5A}*/
    };

/*private void Start()
{
    StartCoroutine(Test("ESTA ES UNA PRUEBA"));
}*/

public void KeyDown(char key)
    {
        byte keycode = (byte)virtualKeyCodes[key];
        keybd_event(keycode, (byte)0x02, 0, 0);

        StartCoroutine(KeyUp(keycode));  

        //Thread.Sleep(10);
        //keybd_event(keycode, (byte)0x82, (uint)0x2, 0);
    }

    private IEnumerator KeyUp(byte keycode)
    {
        yield return new WaitForEndOfFrame();

        //Thread.Sleep(250);

        keybd_event(keycode, (byte)0x82, (uint)0x2, 0);
    }

    private IEnumerator Test(string test)
    {
        yield return new WaitForSeconds(3f);
        
        for (int i = 0; i < test.Length; i++)
        {
            KeyDown(test[i]);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
