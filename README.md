# MiProyectoWeb
#  Comisariato Express - Administración MVC Core

Este sistema web es un módulo administrativo híbrido e inteligente diseñado en **ASP.NET Core (Razor Pages)**. Permite la gestión y adquisición de alimentos en tiempo real mediante un control estricto de accesos y flujos optimizados para el usuario final.



##  Características Clave

* **Validación de Datos Sensibles en el Servidor (Back-End):** Implementación de seguridad crítica mediante la validación algorítmica real del dígito verificador para **Cédulas de Identidad de Ecuador** en C# antes de procesar o guardar los registros de proveedores.
* **Dropdowns Anidados Dinámicos (AJAX):** Eliminación de inputs manuales para claves foráneas. El formulario asocia de forma dinámica las **Categorías** con sus respectivas **Subcategorías** de alimentos mediante peticiones asíncronas (`Fetch API`) al servidor.
* **Control de Acceso basado en Roles:** Rutas protegidas mediante sesiones de servidor para diferenciar las vistas de **Administrador** (gestión de stock) y **Usuario** (catálogo de compras y carrito).

---

## 🛠️ Tecnologías y Arquitectura

* **Backend:** C# / .NET Core 8.0 (Arquitectura limpia bajo Razor Pages)
* **Frontend:** HTML5, CSS3, Bootstrap 5 y Javascript Moderno (AJAX)
* **Persistencia:** Sesiones HTTP seguras en memoria del servidor
* **Estructura de Entrada:** Servidor web integrado en paralelo con interfaz de Windows Forms en el mismo ciclo de ejecución del programa.

---

## 📦 Enlaces del Proyecto

* **Repositorio de Código:** `
* **Aplicación Desplegada:** 
