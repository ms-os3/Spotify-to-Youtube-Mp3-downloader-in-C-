# Offline Streamer

Offline Streamer is an ASP.NET Web Forms application built with C# that allows users to authenticate with Spotify, search for tracks, and download songs from YouTube for offline listening.

> **Note:** Despite the name, this application does **not** stream music. It downloads audio from YouTube based on the selected Spotify track.

---

## Features

- Spotify Authentication
- Spotify Track Search
- YouTube Audio Download
- Offline Music Downloads
- ASP.NET Web Forms UI

---

## Tech Stack

- ASP.NET Web Forms
- C#
- .NET Framework
- Visual Studio
- Spotify Web API

---

## Prerequisites

Before running the project, install:

- Visual Studio 2019 or later (Visual Studio 2022 recommended)
- .NET Framework (matching the project's target framework)
- IIS Express (included with Visual Studio)
- Git

---

## Installation

### 1. Fork the Repository

Fork this repository to your own GitHub account.

### 2. Clone Your Fork

```bash
git clone https://github.com/<your-username>/<repository-name>.git
cd <repository-name>
```

### 3. Open the Solution

Open:

```
Project.sln
```

using Visual Studio.

---

## Configure Spotify API

Create a Spotify application from the Spotify Developer Dashboard.

Open:

```
Offline_Streamer/Web.config
```

Locate the following section and replace the empty values:

```xml
<appSettings>
    <add key="SpotifyClientID" value="YOUR_CLIENT_ID" />
    <add key="SpotifyClientSecret" value="YOUR_CLIENT_SECRET" />
    <add key="SpotifyRedirectUri" value="http://127.0.0.1:5000/Downloads.aspx" />
</appSettings>
```

The Redirect URI configured in your Spotify Developer Dashboard **must exactly match**:

```
http://127.0.0.1:5000/Downloads.aspx
```

---

## Restore Packages

Visual Studio should automatically restore NuGet packages.

If not install them manually

---

## Running the Project

1. Set **Offline_Streamer** as the Startup Project.
2. Press **F5** (or **Ctrl + F5**) to launch the application.
3. IIS Express will start automatically.

---



## Configuration Notes

- Spotify Client ID and Client Secret are required.
- Do not commit your API credentials.
- Ensure the Redirect URI in Spotify Developer Dashboard matches the one configured in `Web.config`.

---

## Disclaimer

This project is intended for educational purposes only.

Users are responsible for complying with YouTube's Terms of Service and all applicable copyright laws when downloading content.

---

## Contributing

1. Fork the repository.
2. Create a new branch.
3. Commit your changes.
4. Push to your fork.
5. Open a Pull Request.

---

## License

Apache License 2.0
