using System;
using UnityEngine;
using UnityEngine.Video;

namespace TerminalAPI
{
    public static partial class TerminalExtensions
    {
        public static void PlayVideoFile(this Terminal terminal, string filepath)
        {
            var url = "file:///" + filepath.Replace('\\', '/');
            terminal.PlayVideoLink(url);
        }

        public static void PlayVideoLink(this Terminal terminal, Uri url)
        {
            terminal.StartCoroutine(TerminalExtensions.PlayVideoLink(url.AbsoluteUri, terminal));
        }

        public static void PlayVideoLink(this Terminal terminal, string url)
        {
            terminal.StartCoroutine(url, terminal);
        }

        private static IEnumerator PlayVideoLink(string url, Terminal terminal)
        {
            yield return new WaitForFixedUpdate();
            terminal.terminalImage.enabled = true;
            terminal.terminalImage.texture = terminal.videoTexture;
            terminal.displayingPersistentImage = null;
            terminal.videoPlayer.clip = null;
            terminal.videoPlayer.source = (VideoSource)1;
            terminal.videoPlayer.url = url;
            terminal.videoPlayer.enabled = true;
        }
    }
}
