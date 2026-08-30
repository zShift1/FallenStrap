# Contribuciones

¡Gracias por querer colaborar con FallenStrap! Esto te guía para que todo sea fluido.

## Código de conducta

Sé respetuoso. Este proyecto es mantenido por gente real en su tiempo libre. Discusiones técnicas, no personales.

## Cómo contribuir

1. **Forkeá** el repositorio y cloná tu fork.
2. Creá una rama con nombre descriptivo: `git checkout -b fix/lo-que-sea`.
3. Hacé tus cambios. Seguí las convenciones de estilo del código existente (C#, WPF, misma indentación).
4. **Probá que compila** antes de abrir el PR:

   ```powershell
   dotnet build FallenStrap\FallenStrap.csproj -c Release
   ```

5. Abrí un Pull Request hacia `main` describiendo:
   - Qué problema resuelve
   - Cómo lo probaste
   - Capturas si cambia la UI

## Qué aportar

Algunas ideas útiles, aunque cualquier mejora es bienvenida:

- Nuevos presets de Fast Flags por juego (¡los juegos cambian todo el tiempo!)
- Fixes de bugs y resiliencia
- Mejoras de traducción
- Documentación

## Reportar bugs

Usá la plantilla de **Bug Report** en Issues e incluí el log de FallenStrap:

```
%LOCALAPPDATA%\FallenStrap\Logs\
```

El log más reciente suele contar toda la historia.

## Dudas

Abrí un issue con etiqueta `question` o pregunta en el hilo que corresponda. No hay preguntas tontas.