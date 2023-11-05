using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public static class SpriteSheetPackerImport
{
    [MenuItem("CustomTools/SpriteSheet/ProcessToSprite")]
    static void ProcessToSprite() 
    {
        Texture2D image = Selection.activeObject as Texture2D;//获取当前Hierarchy面板或Project文件夹下选择的对象Object
        string rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(image));
        string path = rootPath + "/" + image.name + ".PNG";

        //重新导入资源
        TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;
        AssetDatabase.CreateFolder(rootPath, image.name);

        //设置可读
        texImp.isReadable = true;
        //重新导入资源应用设定
        AssetDatabase.ImportAsset(path);

        //遍历小图集
        foreach (SpriteMetaData metaData in texImp.spritesheet)
        {
            var width = (int)metaData.rect.width;
            var height = (int)metaData.rect.height;
            Texture2D smallImage = new Texture2D(width, height);

            for (int y = (int)metaData.rect.y; y < metaData.rect.y + metaData.rect.height; y++) 
            {
                for (int x = (int)metaData.rect.x; x < metaData.rect.x + metaData.rect.width; x++)
                {
                    //这一通操作下来，那么SetPixel的值就是图片本身的大小了（width,height)
                    smallImage.SetPixel(x-(int)metaData.rect.x, y-(int)metaData.rect.y, image.GetPixel(x, y));
                    Debug.Log("Still Doing");
                }
            }

            //转换纹理
            //if (smallImage.format != TextureFormat.ARGB32 && smallImage.format != TextureFormat.RGB24)
            //{
            //    Texture2D newTexture = new Texture2D(smallImage.width, smallImage.height);
            //    newTexture.SetPixels(smallImage.GetPixels(0), 0);
            //    smallImage = newTexture;
            //}
            var pngData = smallImage.EncodeToPNG();
            File.WriteAllBytes(rootPath + "/" + image.name + "/" + metaData.name + ".PNG", pngData);
        }
        Debug.Log("Trans pics done");
    }
}

