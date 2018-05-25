# Google Drive Upload Tool
Una herramienta para subir archivos a Google Drive. El programa resume las subidas en caso de que ocurra una falla. Es perfecto para subir archivos grandes o si tu conexión es inestable. Adicionalmente, los archivos preservarán su fecha de modificación. También, puedes navegar en tu Drive y subir archivos y carpetas a un lugar determinado.

# ¿Cómo usarlo?
1. Antes de usar este programa, siga el paso 1 de esta guía de Google: [https://developers.google.com/drive/v3/web/quickstart/dotnet](https://developers.google.com/drive/v3/web/quickstart/dotnet).
2. Luego, lanza el programa. Tendrás que autorizar tu cuenta para poder usar el programa.
3. ¡Comienza a subir y descargar archivos!

# ¿Cómo subo un archivo o carpeta?
1. Simplemente, arrastre el archivo o carpeta al programa.
2. Presione el botón "Subir".

**-O-**

1. Presiona en Archivo -> Subir -> Archivo(s)/Carpeta
2. Presione el botón "Subir"

* El paso 2 se puede automatizar activando la opción "Subir archivos automáticamente" en el menú "Opciones"
* Las carpetas subirán con su estructura original.
* Si un archivo es interrumpido, usted podrá resumir la subida desde el punto en que la subida fue interumpida.

# ¿Como selecciono una carpeta?
* Simplemente, navega a la carpeta donde desee subir sus archivos.
* Para cambiar la carpeta a subir de un archivo que ya está en la lista de subida, seleccione la carpeta en el navegador de carpetas y presione el botón "Subir archivo(s) a esta carpeta".

# ¿Cómo creo una carpeta?
1. Navegue al lugar donde desee crear la carpeta nueva.
2. Presione en "Crear Carpeta" o ve a Acciones -> Crear Carpeta.
3. Escriba el nombre de la carpeta nueva y presione OK.

# ¿Cómo verifico si el ID de la carpeta está correcta?
1. Escriba el ID de la carpeta en el blanco debajo del texto "Subir a este ID de directorio..."
2. PResione el botón titulado "Obtener Nombre de la Carpeta"
3. Si el ID es correcto, verás el nombre a la izquierda del botón. Si el ID es incorrecto, verás un mensaje de error notificando que el ID es incorrecto.

# ¿Cómo descargo un archivo?
1. Seleccione un archivo en la lista de archivos.
2. Presione el botón "Descargar", presione ALT + D en su teclado, o vaya a Archivo -> Descargar -> Archivo(s) seleccionado(s).
3. Escoja una carpeta para descargar su archivo y presione en "Save".
4. La descarga comenzará.

# ¿Puedo descargar más de un archivo?
¡Absolutamente! Simplemente, seleccione los archivos y presione las teclas ALT + D en su teclado, o vaya a Archivo -> Descargar -> Archivo(s) seleccionado(s). Luego, seleccione una carpeta para descargar sus archivos.

# ¿Puedo descargar una carpeta y todo su contenido?
¡Absolutamente! Simplemente, seleccione la carpeta y presione las teclas ALT + D en su teclado, o vaya a Archivo -> Descargar -> Carpeta seleccionada. Luego, seleccione una carpeta para descargar la carpeta y todo su contenido.

# ¿Cómo guardo el checksum de un archivo?
1. Seleccione el archivo.
2. Presione en "Guardar archivo MD5", presione ALT + C en su teclado, o vaya a Acciones -> Guardar Checksums -> Archivo(s) Seleccionado(s).
3. Busque un lugar para guardar el archivo.

# ¿Cómo guardo el checksum de más de un archivo?
1. Seleccione los archivo presionando la tecla Ctrl o Shift en su teclado, o presione ALT + A para seleccionar todos los archivos.
2. Presione el botón "Guardar Checksums de los archivos", presione ALT + C en su teclado,  o vaya a Acciones -> Guardar Checksums -> Archivo(s) Seleccionado(s).
3. Busque un lugar para guardar el archivo.

# ¿Cómo guardo el checksum del contenido de una carpeta?
1. Seleccione la carpeta para guardar los checksums de su contenido.
2. Presione ALT + C en su teclado, o vaya a Acciones -> Guardar Checksums -> Carpeta Seleccionada.
3. Busque un lugar para guardar el archivo. 

Los checksums utilizados son los que Google Drive genera y almacena.

# ¿Cómo envío uno o más archivos o carpetas a la basura?
1. Seleccione el/los archivo(s) o carpeta(s) a enviar a la basura. 
2. Presione la tecla "Delete" en su teclado o vaya a Acciones -> Mover a la Basura -> Archivo(s) Seleccionados o Carpeta(s) Seleccionada(s)

# ¿Cómo restauro uno o más archivos o carpetas de la basura?
1. Presione el botón "Ver Basura"
1. Seleccione el/los archivo(s) o carpeta(s) a restaurar
2. Presione las teclas ALT + R en su teclado o vaya a Acciones -> Restaurar -> Archivo(s) Seleccionados o Carpeta(s) Seleccionada(s)

# ¿Cómo puedo abrir un archivo en la págna de Google Drive desde la aplicación?
Símplemente, haz doble click en el archivo. Serás dirigido a la página del archivo Google Drive.

# ¿Cómo puedo abrir una carpeta en la página de Google Drive desde la aplicación?
Presione la tecla SHIFT y haga doble click en la carpeta. Serás dirigido a la página de la carpeta en Google Drive.

# ¿Qué hace la opción "Copiar archivo a memoria antes de subirlo si hay memoria disponible"?
Si tienes memoria RAM que no está en uso en tu sistema, y el archivo que quieres subir cabe en la misma, el programa copiará el archivo a la memoria. Luego, el archivo será leido de la memoria en vez de el disco donde se encuentra. Esto puede hacer que tus archivos suban más rápido aunque la copia inicial puede ser lenta. esto es debido a que el archivo se leerá desde la RAM en vez del disco duro. También, esta opción es util si estás usando un disco duro externo, ya que una vez el archivo se copie a la memoria, puedes desconectar el mismo y el programa seguirá subiendo el archivo. Nota que debe estar conectado si tienes más archivos pues sólo se copia el archivo que se está subiendo actualmente.

# ¿Qué hace la opción "Especificar tamaño de pedazos"?
Esta opción abrirá una ventana en donde puedes especificar un tamaño que se usará para subir tus archivos en pedazos. Google Drive sube tus archivos en pedazos de 256KB (El programa los sube en pedazos de 1MB). Usando esta opción, puedes definir el tamaño deseado. Un valor pequeño es bueno si tienes una velocidad de Subida ("Upload") lento, pero si tu velocidad es alta, usar un número alto puede hacer que se utilice mejor la misma. Nota que si no usas la opción de "Copiar archivo a memoria antes de subirlo si hay memoria disponible" y el tamaño de pedazos es grande, es posible que notes algunas pausas en el proceso de subir el archivo, pues cuando se termina de subir un pedazo, el programa debe leer los proximos KB/MB del disco duro/ssd. Esta pausa es normal.

# Shortcuts del teclado:
* **Enter**: Entrar en carpeta.
* **Delete**: Mover archivo o carpeta a la basura.
* **F5**: Refrescar la lista de archivos y carpetas.
* **CTRL + A**: Seleccionar todos los archivos o carpetas.
* **CTRL + C**: Guardar checksum de uno o más archivos o el contenido de una carpeta.
* **CTRL + D**: Descargar uno o más archivos o el contenido de una carpeta.
* **CTRL + R**: Cuando no estás en la basura, renombrar un archivo o carpeta. Cuando estás en la basura, restaura el archivo o carpeta.

# ¿Como abrir y compilar este proyecto?
Este proyecto fue escrito usando Visual Studio 2017, en el lenguaje Visual Basic .NET. Necesitarás tener los componentes de Windows Desktop instalado para poder abrir y editar este proyecto. También, necesitarás las APIs de Google Drive los cuales están disponibles en el NuGet Package Manager.

La función "Copiar archivo a memoria antes de subirlo" depende de un código escrito en C# llamado MemoryTributary. Puedes obtener el mismo aquí: https://www.codeproject.com/articles/348590/a-replacement-for-memorystream. Luego, crea un DLL y añádelo a este proyecto.

¡Disfruten!

# Historial de cambios:
Pueden ver el historial de cambios [presionando aquí](https://github.com/moisesmcardona/GoogleDriveUploadTool/blob/master/Google%20Drive%20Uploader1/Changelog.txt).

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
