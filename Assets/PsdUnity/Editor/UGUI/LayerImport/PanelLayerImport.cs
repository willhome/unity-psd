﻿using System;
using UnityEngine;
using PSDUnity;
namespace PSDUnity.UGUI
{
    public class PanelLayerImport : ILayerImport
    {
        PSDImportCtrl ctrl;
        public PanelLayerImport(PSDImportCtrl ctrl)
        {
            this.ctrl = ctrl;
        }

        public UGUINode DrawLayer(GroupNode layer, UGUINode parent)
        {
            UGUINode node = PSDImporter.InstantiateItem(GroupType.IMAGE, layer.Name, parent);//GameObject.Instantiate(temp) as UnityEngine.UI.Image;
            UnityEngine.UI.Image panel = node.InitComponent<UnityEngine.UI.Image>();

            if(layer.children!=null)
                ctrl.DrawLayers(layer.children.ConvertAll(x=>x as GroupNode).ToArray(), node);//子节点

            bool havebg = false;
            for (int i = 0; i < layer.images.Count; i++)
            {
                ImgNode image = layer.images[i];

                if (image.Name.ToLower().StartsWith("b_"))
                {
                    havebg = true;
                    PSDImporter.SetPictureOrLoadColor(image, panel);
                    PSDImporter.SetRectTransform(image, panel.GetComponent<RectTransform>());
                    panel.name = layer.Name;
                }
                else
                {
                    ctrl.DrawImage(image, node);
                }
            }
            if (!havebg)
            {
                PSDImporter.SetRectTransform(layer, panel.GetComponent<RectTransform>());
                Color color;
                if (ColorUtility.TryParseHtmlString("#FFFFFF01", out color))
                {
                    panel.GetComponent<UnityEngine.UI.Image>().color = color;
                }
                panel.name = layer.Name;
            }
            return node;

        }

    }
}