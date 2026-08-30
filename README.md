> [!CAUTION]
> El único lugar oficial para descargar FallenStrap es **este repositorio de GitHub**. Cualquier otra página que ofrezca descargas o se haga pasar por este proyecto no es nuestra.

<p align="center">
    <img src="https://github.com/zShift1/FallenStrap/raw/main/Images/FallenStrap-full-dark.png#gh-dark-mode-only" width="400">
    <img src="https://github.com/zShift1/FallenStrap/raw/main/Images/FallenStrap-full-light.png#gh-light-mode-only" width="400">
</p>

<div align="center">

**El bootstrapper de Roblox, a tu manera.**

[![License][shield-repo-license]][repo-license]
[![Build][shield-repo-workflow]][repo-actions]
[![Downloads][shield-repo-releases]][repo-releases]
[![Versión][shield-repo-latest]][repo-latest]
[![Plataforma][shield-platform]][repo-url]
[![.NET][shield-dotnet]][repo-url]

</div>

---

FallenStrap es un reemplazo de terceros para el bootstrapper estándar de Roblox, con características y mejoras adicionales, un tema claro y una experiencia renovada.

**Solo compatible con Windows 10/11 (x64).** ¿Tienes un problema o necesitas ayuda? [Abre un issue](https://github.com/zShift1/FallenStrap/issues).

## ✨ Características

- 🎮 **Discord Rich Presence** — tus amigos ven exactamente qué estás jugando, con ícono del juego y botón para unirse
- ⚡ **Presets de Fast Flags por juego** — las flags que mejor funcionan hoy en Blox Fruits, Arsenal, Brookhaven, Adopt Me!, MM2 y Doors, aplicadas con un clic
- 🎨 **Tema claro** con animación de fondo al cambiar de tema
- 📊 **Calidad gráfica y resolución** configurables desde la página de Inicio
- 🖱️ **Cursor personalizado** sin necesidad de mods externos
- 🗺️ **Ubicación del servidor** — enterate dónde está geográficamente el server al que te conectás (cortesía de [ipinfo.io](https://ipinfo.io))
- 🛠️ **Modding de archivos de contenido** — sonido de muerte, cursors, fuentes y más, sin tocar el cliente
- 🚀 **Actualizaciones automáticas del cliente** de Roblox

## ⚙️ Instalación

1. Descargá el `.exe` de la [última versión](https://github.com/zShift1/FallenStrap/releases/latest).
2. Ejecutalo. Configurá tus preferencias si querés, e instalá.
3. Listo. Se añade al menú de Inicio, donde podés reabrir el menú y cambiar todo cuando quieras.

> [!NOTE]
> **No requiere nada más.** El ejecutable es autocontenido: no necesitás instalar el .NET Runtime, Roblox ya instalado, ni Discord Desktop. FallenStrap funciona completo con Roblox solo.

Es probable que Windows SmartScreen muestre un aviso la primera vez. Es normal en programas nuevos y firmados por particulares: hacé clic en **"Más información"** y luego en **"Ejecutar de todas formas"**.

## ❓ Preguntas frecuentes

**P: ¿Necesito tener Discord Desktop instalado?**

**R:** No. FallenStrap funciona igual sin Discord. La presencia en Discord es opcional: si Discord está abierto, la mostrás; si no, la app ignora la conexión y sigue normal. Nada se rompe ni se bloquea.

**P: ¿Esto es malware?**

**R:** No. El código fuente es visible para todos, y sería imposible colar algo malicioso en las descargas sin que nadie lo notara. Solo asegurate de descargarlo de una fuente oficial: este repositorio de GitHub.

**P: ¿Pueden banearme por usarlo?**

**R:** No, no debería. FallenStrap no interactúa con el cliente de Roblox de la misma forma que lo hacen los exploits. Es una modificación del lanzador, no del juego.

**P: ¿Sirve para mejorar los FPS?**

**R:** Depende. Configurando los presets de Fast Flags y la fidelidad gráfica podés lograr un rendimiento más estable en muchos juegos, especialmente en PCs de gama media. No hay milagros: los resultados varían por hardware y por juego.

## 🛠️ Compilar desde el código

Requisitos: [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) (o superior).

```powershell
dotnet publish FallenStrap\FallenStrap.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

El ejecutable compilado queda en la carpeta de publicación, listo para instalar.

## 📄 Licencia

FallenStrap es software libre bajo la licencia **MIT**.

## 🙏 Agradecimientos

- [lepoco](https://github.com/lepoco) y [WPF UI](https://github.com/lepoco/wpfui), la librería de UI usada en este proyecto

[shield-repo-license]:  https://img.shields.io/github/license/zShift1/FallenStrap?color=981bfe
[shield-repo-workflow]: https://img.shields.io/github/actions/workflow/status/zShift1/FallenStrap/ci-release.yml?branch=main&label=builds
[shield-repo-releases]: https://img.shields.io/github/downloads/zShift1/FallenStrap/latest/total?color=7a39fb
[shield-repo-latest]:   https://img.shields.io/github/v/release/zShift1/FallenStrap?color=7a39fb
[shield-platform]:      https://img.shields.io/badge/plataforma-Windows_10%2F11_%E2%80%93_x64-5a7dfa
[shield-dotnet]:        https://img.shields.io/badge/.NET-6.0-512bd4

[repo-url]:     https://github.com/zShift1/FallenStrap
[repo-license]: https://github.com/zShift1/FallenStrap/blob/main/LICENSE
[repo-actions]: https://github.com/zShift1/FallenStrap/actions
[repo-releases]: https://github.com/zShift1/FallenStrap/releases
[repo-latest]:  https://github.com/zShift1/FallenStrap/releases/latest