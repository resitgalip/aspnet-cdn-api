# Asp.Net CDN  :+1:

>**Motivation**

Wouldn't it be nice to be able to write your cdn web service? 
This project provides you to make cdn service that supports separate websites. <br><br>

**First step : Set your project url in appsettings.json**

>**Properties**
 - Swagger Support
 - Check real image with System.Drawing.Image.
 - Save images in separate website folders.
 - Subfolder support.<br>

<br><br>
>**There are two methods that you can use.**

1. **UploadImage**
- This method use POST method to save your image. 
- Encrypt file name with Guid.
 
2. **GetImage**
 - This method use GET method to get your image. 
 - Supports downloading image from source.
 - Supports image resizing.
 - Supports image cache.
  <br><br> <br>
  


## Examples

**Upload Image**<br>
<kbd>
<img width="800" alt="upload" src="https://user-images.githubusercontent.com/57272527/197370175-9ee0ee0a-0206-4388-b94d-3ae32b2ccc48.png">
</kbd>
<br><br>
**Get Image**<br>
<kbd>
<img width="800" alt="getimage" src="https://user-images.githubusercontent.com/57272527/197370179-1214945a-3d5e-4d4a-a895-7b68e369c22e.png">
</kbd>
<br><br>
**Resize Image**<br>
Resize image can get uploaded images. Just write -1000x1000 before image extension.<br>
For example : imagename.jpg => imagename-100x150.jpg<br><br>
<kbd>
<img width="800" alt="getresizingimage" src="https://user-images.githubusercontent.com/57272527/197370181-0f5a3b84-3381-45c7-a599-6553fc3f68c1.png">
</kbd>




Asp.NET CDN Api Project
