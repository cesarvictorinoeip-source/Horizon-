# Horizon FutureVest2
# 🌎 Horizon FutureVest

Aplicación web desarrollada con **ASP.NET Core MVC** y **Entity Framework Core** que ayuda a un analista a decidir en qué países es más atractivo invertir. A partir de indicadores macroeconómicos registrados por año, calcula un **ranking de países** con un modelo de **scoring ponderado** y estima la **tasa de retorno** de cada uno.

> Proyecto académico desarrollado en el Instituto Tecnológico de Las Américas (ITLA).

---

## ✨ Funcionalidades

### 📊 Ranking de países
- Selección del año a analizar (por defecto, el más reciente con datos).
- Validación de que los pesos de los macroindicadores sumen exactamente **1**.
- Solo entran al ranking los países que tienen datos de **todos** los macroindicadores con peso mayor a 0 en ese año.
- Se requieren al menos **dos países elegibles** para generar el ranking.
- Resultado ordenado de mayor a menor scoring, con el nombre del país, su código ISO, el scoring y la tasa de retorno estimada.

### 🧪 Simulador de ranking
- Permite probar **pesos distintos** para los macroindicadores sin modificar los valores originales.
- Se pueden elegir solo los macroindicadores que el analista quiera incluir en la simulación.
- Usa las mismas validaciones y cálculos que el ranking principal.

### 🗂️ Mantenimientos
- **Países:** nombre y código ISO.
- **Macroindicadores:** nombre, peso y si un valor más alto es mejor o peor (por ejemplo, PIB vs. inflación). La suma de pesos nunca puede superar 1.
- **Indicadores por país:** valor de cada macroindicador por país y por año, con filtros por país y año. No se permite registrar el mismo macroindicador dos veces para un mismo país y año.
- **Tasas de retorno:** configuración de la tasa mínima y máxima estimada (por defecto, 2 % y 15 %).

---

## 🧮 Modelo de cálculo

1. **Normalización min-max** de cada indicador entre los países elegibles:
   - Si un valor más alto es mejor: `(valor − mínimo) / (máximo − mínimo)`
   - Si un valor más bajo es mejor: `(máximo − valor) / (máximo − mínimo)`
   - Si el mínimo y el máximo son iguales, el valor normalizado es **0.5**.

2. **Sub-puntaje** de cada indicador: `valor normalizado × peso del macroindicador`.

3. **Scoring del país**: suma de todos sus sub-puntajes (siempre entre 0 y 1).

4. **Tasa de retorno estimada**: `tasa mínima + (tasa máxima − tasa mínima) × scoring`.

**Ejemplo:** con tasas de 2 % y 15 %, un país con scoring de 0.70 tendría una tasa de retorno estimada de **11.1 %**.

---

## 🛠️ Tecnologías

| Área | Tecnologías |
|---|---|
| Backend | C#, ASP.NET Core MVC |
| Base de datos | SQL Server, Entity Framework Core (Code First) |
| Transferencia de datos | DTOs |
| Validaciones | ViewModels con Data Annotations |
| Interfaz | Bootstrap |

---

## 🏗️ Estructura

El proyecto está organizado en tres capas:

- **Horizon FutureVest** → Aplicación web MVC: controladores, vistas y ViewModels.
- **Application** → Lógica de negocio: servicios, DTOs y los cálculos del ranking.
- **Persistence** → Acceso a datos: DbContext, entidades y migraciones.

---

## 🚀 Cómo ejecutarlo

### Requisitos
- [.NET SDK](https://dotnet.microsoft.com/download) (versión del proyecto)
- SQL Server
- Visual Studio 2022 (recomendado)

### Pasos

1. **Clona el repositorio**
   ```bash
   git clone https://github.com/cesarvictorinoeip-source/HorizonFutureVest.git
   ```

2. **Configura la cadena de conexión** en el `appsettings.json` del proyecto web, apuntando a tu servidor de SQL Server.

3. **Aplica las migraciones** desde la Consola del Administrador de paquetes, seleccionando **Persistence** como proyecto predeterminado:
   ```powershell
   Update-Database
   ```

4. **Ejecuta el proyecto** `Horizon FutureVest`.

### Primeros pasos en la app
1. Registra algunos **países**.
2. Crea los **macroindicadores** con pesos que sumen 1 (por ejemplo, PIB 0.6 e inflación 0.4).
3. Agrega los **indicadores** de cada país para un mismo año.
4. Ve al **Home** y genera el ranking.

---

## 👨‍💻 Autor

**César Tomás Victorino**
Estudiante de Desarrollo de Software — ITLA

- GitHub: [@cesarvictorinoeip-source](https://github.com/cesarvictorinoeip-source)
