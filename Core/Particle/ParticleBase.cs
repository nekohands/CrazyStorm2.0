﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CrazyStorm.Core
{
    public struct ParticleBaseData : IFieldData
    {
        public int maxLife;
        public int currentFrame;
        public Vector2 pposition;
        public float widthScale;
        public RGB rgb;
        public float mass;
        public float opacity;
        public float pspeed;
        public float pspeedAngle;
        public float pacspeed;
        public float pacspeedAngle;
        public float protation;
        public BlendType blendType;
        public bool killOutside;
        public bool collision;
        public bool ignoreMask;
        public bool ignoreRebound;
        public bool ignoreForce;
        public bool fogEffect;
        public bool fadeEffect;
        public void SetField(int fieldIndex, ValueType value)
        {
            throw new NotImplementedException();
        }
        public ValueType GetField(int fieldIndex)
        {
            throw new NotImplementedException();
        }
    }
    public abstract class ParticleBase : PropertyContainer, IXmlData, IRebuildReference<ParticleType>
    {
        #region Private Members
        ParticleType type;
        int typeID = -1;
        ParticleBaseData particleBaseData;
        #endregion

        #region Public Members
        [IntProperty(0, int.MaxValue)]
        public int MaxLife
        {
            get { return particleBaseData.maxLife; }
            set { particleBaseData.maxLife = value; }
        }
        [RuntimeProperty]
        public int CurrentFrame
        {
            get { return particleBaseData.currentFrame; }
            set { particleBaseData.currentFrame = value; }
        }
        [RuntimeProperty]
        public Vector2 PPosition
        {
            get { return particleBaseData.pposition; }
            set { particleBaseData.pposition = value; }
        }
        public ParticleType Type
        {
            get { return type; }
            set { type = value; }
        }
        [FloatProperty(0, float.MaxValue)]
        public float WidthScale
        {
            get { return particleBaseData.widthScale; }
            set { particleBaseData.widthScale = value; }
        }
        [RGBProperty]
        public RGB RGB
        {
            get { return particleBaseData.rgb; }
            set { particleBaseData.rgb = value; }
        }
        [FloatProperty(0, float.MaxValue)]
        public float Mass
        {
            get { return particleBaseData.mass; }
            set { particleBaseData.mass = value; }
        }
        [FloatProperty(0, float.MaxValue)]
        public float Opacity
        {
            get { return particleBaseData.opacity; }
            set { particleBaseData.opacity = value; }
        }
        [FloatProperty(float.MinValue, float.MaxValue)]
        public float PSpeed
        {
            get { return particleBaseData.pspeed; }
            set { particleBaseData.pspeed = value; }
        }
        [FloatProperty(float.MinValue, float.MaxValue)]
        public float PSpeedAngle
        {
            get { return particleBaseData.pspeedAngle; }
            set { particleBaseData.pspeedAngle = value; }
        }
        [FloatProperty(float.MinValue, float.MaxValue)]
        public float PAcspeed
        {
            get { return particleBaseData.pacspeed; }
            set { particleBaseData.pacspeed = value; }
        }
        [FloatProperty(float.MinValue, float.MaxValue)]
        public float PAcspeedAngle
        {
            get { return particleBaseData.pacspeedAngle; }
            set { particleBaseData.pacspeedAngle = value; }
        }
        [FloatProperty(float.MinValue, float.MaxValue)]
        public float PRotation
        {
            get { return particleBaseData.protation; }
            set { particleBaseData.protation = value; }
        }
        [EnumProperty(typeof(BlendType))]
        public BlendType BlendType
        {
            get { return particleBaseData.blendType; }
            set { particleBaseData.blendType = value; }
        }
        [BoolProperty]
        public bool KillOutside
        {
            get { return particleBaseData.killOutside; }
            set { particleBaseData.killOutside = value; }
        }
        [BoolProperty]
        public bool Collision
        {
            get { return particleBaseData.collision; }
            set { particleBaseData.collision = value; }
        }
        [BoolProperty]
        public bool IgnoreMask
        {
            get { return particleBaseData.ignoreMask; }
            set { particleBaseData.ignoreMask = value; }
        }
        [BoolProperty]
        public bool IgnoreRebound
        {
            get { return particleBaseData.ignoreRebound; }
            set { particleBaseData.ignoreRebound = value; }
        }
        [BoolProperty]
        public bool IgnoreForce
        {
            get { return particleBaseData.ignoreForce; }
            set { particleBaseData.ignoreForce = value; }
        }
        #endregion

        #region Constructor
        public ParticleBase()
        {
            particleBaseData.maxLife = 200;
            particleBaseData.widthScale = 1;
            particleBaseData.rgb = new RGB(255, 255, 255);
            particleBaseData.mass = 1;
            particleBaseData.opacity = 100;
            particleBaseData.pspeed = 5;
            particleBaseData.killOutside = true;
            particleBaseData.collision = true;
            particleBaseData.fogEffect = true;
            particleBaseData.fadeEffect = true;
        }
        #endregion

        #region Public Methods
        public virtual XmlElement BuildFromXml(XmlElement node)
        {
            var nodeName = "ParticleBase";
            var particleBaseNode = (XmlElement)node.SelectSingleNode(nodeName);
            if (node.Name == nodeName)
                particleBaseNode = node;

            //properties
            base.BuildFromXmlElement(particleBaseNode);
            //type
            if (particleBaseNode.HasAttribute("type"))
            {
                string typeAttribute = particleBaseNode.GetAttribute("type");
                int parsedID;
                if (int.TryParse(typeAttribute, out parsedID))
                    typeID = parsedID;
                else
                    throw new System.IO.FileLoadException("FileDataError");
            }
            //particleBaseData
            XmlHelper.BuildStruct(ref particleBaseData, particleBaseNode, "ParticleBaseData");
            return particleBaseNode;
        }
        public virtual XmlElement StoreAsXml(XmlDocument doc, XmlElement node)
        {
            var particleBaseNode = doc.CreateElement("ParticleBase");
            //properties
            particleBaseNode.AppendChild(base.GetXmlElement(doc));
            //type
            if (type != null)
            {
                var typeAttribute = doc.CreateAttribute("type");
                typeAttribute.Value = type.ID.ToString();
                particleBaseNode.Attributes.Append(typeAttribute);
            }
            //particleBaseData
            XmlHelper.StoreStruct(particleBaseData, doc, particleBaseNode, "ParticleBaseData");
            node.AppendChild(particleBaseNode);
            return particleBaseNode;
        }
        public void RebuildReferenceFromCollection(IList<ParticleType> collection)
        {
            //type
            if (typeID != -1)
            {
                foreach (var target in collection)
                {
                    if (typeID == target.ID)
                    {
                        type = target;
                        break;
                    }
                }
                typeID = -1;
            }
        }
        #endregion
    }
}