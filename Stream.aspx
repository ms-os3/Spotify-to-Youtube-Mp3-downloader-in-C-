<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Stream.aspx.cs" Inherits="Offline_Streamer.Stream" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .player-wrap { max-width: 900px; margin: 24px auto; font-family: Segoe UI, sans-serif; }
        .card { background: #121820; color: #f1f5f9; border-radius: 12px; padding: 18px; box-shadow: 0 6px 20px rgba(0,0,0,.25); }
        .title { font-size: 20px; font-weight: 600; margin-bottom: 10px; }
        .now { font-size: 14px; color: #94a3b8; margin-bottom: 12px; min-height: 20px; }
        .controls { display: flex; gap: 10px; margin-bottom: 14px; }
        .btn { border: 0; border-radius: 10px; padding: 10px 14px; cursor: pointer; background: #1f2937; color: #fff; }
        .btn:hover { background: #334155; }
        .btn.shuffle.on { background: #0f766e; }
        audio { width: 100%; margin-bottom: 16px; }
        .list { list-style: none; padding: 0; margin: 0; max-height: 420px; overflow-y: auto; }
        .list li { padding: 10px 12px; border-radius: 8px; margin-bottom: 6px; background: #0b1220; cursor: pointer; }
        .list li:hover { background: #162236; }
        .list li.active { background: #1d4ed8; }
        .empty { color: #cbd5e1; }
    </style>

    <div class="player-wrap">
        <div class="card">
            <div class="title">Offline Stream</div>
            <div id="nowPlaying" class="now">Select a song</div>

            <div class="controls">
                <button type="button" id="btnPrev" class="btn">Prev</button>
                <button type="button" id="btnPlayPause" class="btn">Play/Pause</button>
                <button type="button" id="btnNext" class="btn">Next</button>
                <button type="button" id="btnShuffle" class="btn shuffle">Shuffle: Off</button>
            </div>

            <audio id="audioPlayer" controls preload="metadata"></audio>

            <asp:Repeater ID="SongsRepeater" runat="server">
                <HeaderTemplate><ul id="playlist" class="list"></HeaderTemplate>
                <ItemTemplate>
                    <li data-src='<%# Eval("Url") %>' data-title='<%# Eval("Title") %>'>
                        <%# Eval("Title") %>
                    </li>
                </ItemTemplate>
                <FooterTemplate></ul></FooterTemplate>
            </asp:Repeater>

            <asp:Panel ID="EmptyPanel" runat="server" Visible="false" CssClass="empty">
                No MP3 files found in App_Data/Songs.
            </asp:Panel>
        </div>
    </div>

    <script>
        (function () {
            const audio = document.getElementById('audioPlayer');
            const now = document.getElementById('nowPlaying');
            const listItems = Array.from(document.querySelectorAll('#playlist li'));
            const btnPrev = document.getElementById('btnPrev');
            const btnNext = document.getElementById('btnNext');
            const btnPlayPause = document.getElementById('btnPlayPause');
            const btnShuffle = document.getElementById('btnShuffle');

            if (!listItems.length) return;

            let currentIndex = 0;
            let shuffle = false;

            function markActive() {
                listItems.forEach((li, i) => li.classList.toggle('active', i === currentIndex));
            }

            function loadAndPlay(i, autoplay) {
                currentIndex = (i + listItems.length) % listItems.length;
                const item = listItems[currentIndex];
                audio.src = item.dataset.src;
                now.textContent = "Now Playing: " + item.dataset.title;
                markActive();
                if (autoplay) audio.play();
            }

            function nextIndex() {
                if (!shuffle) return (currentIndex + 1) % listItems.length;
                if (listItems.length === 1) return 0;
                let n;
                do { n = Math.floor(Math.random() * listItems.length); } while (n === currentIndex);
                return n;
            }

            btnPlayPause.addEventListener('click', function () {
                if (!audio.src) loadAndPlay(0, true);
                else if (audio.paused) audio.play();
                else audio.pause();
            });

            btnPrev.addEventListener('click', function () {
                loadAndPlay(currentIndex - 1, true);
            });

            btnNext.addEventListener('click', function () {
                loadAndPlay(nextIndex(), true);
            });

            btnShuffle.addEventListener('click', function () {
                shuffle = !shuffle;
                btnShuffle.classList.toggle('on', shuffle);
                btnShuffle.textContent = "Shuffle: " + (shuffle ? "On" : "Off");
            });

            listItems.forEach((li, i) => {
                li.addEventListener('click', function () { loadAndPlay(i, true); });
            });

            audio.addEventListener('ended', function () {
                loadAndPlay(nextIndex(), true);
            });

            loadAndPlay(0, false);
        })();
    </script>
</asp:Content>
