using Framework;
using UnityEngine;

namespace Library.Extensions
{
    public static class StringExtensions
    {
        public static string Bold(this string str) => "<b>" + str + "</b>";
        public static string Italic(this string str) => "<i>" + str + "</i>";
        public static string Size(this string str, int size) => $"<size={size}>{str}</size>";

        public static string Color(this string str, string clr) => $"<color={clr}>{str}</color>";
        public static string Color(this string str, Color clr) => $"<color=#{clr.ToHex()}>{str}</color>";

        public static string Red(this string str) => $"<color=#{UnityEngine.Color.red.ToHex()}>{str}</color>";
        public static string Green(this string str) => $"<color=#{UnityEngine.Color.green.ToHex()}>{str}</color>";
        public static string Blue(this string str) => $"<color=#{UnityEngine.Color.blue.ToHex()}>{str}</color>";
        public static string Yellow(this string str) => $"<color=#{UnityEngine.Color.yellow.ToHex()}>{str}</color>";
        public static string Magenta(this string str) => $"<color=#{UnityEngine.Color.magenta.ToHex()}>{str}</color>";
        public static string Cyan(this string str) => $"<color=#{UnityEngine.Color.cyan.ToHex()}>{str}</color>";
    }
}