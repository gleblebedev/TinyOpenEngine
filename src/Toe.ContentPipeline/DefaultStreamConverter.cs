﻿using System;
using System.Collections.Generic;
using System.Numerics;

namespace Toe.ContentPipeline
{
    public partial class StreamConverterFactory : IStreamConverterFactory
    {
        public static readonly StreamConverterFactory Default;

        static StreamConverterFactory()
        {
            Default = new StreamConverterFactory();
            Default.RegisterType<float>(new StreamMetaInfo(typeof(float), typeof(float), 1, 1))
                .RegisterConverter(value => new Vector2(value, default(float)))
                .RegisterConverter(value => new Vector3(value, default(float), default(float)))
                .RegisterConverter(value => new Vector4(value, default(float), default(float), default(float)))
                .RegisterConverter(value => new Vector2d((double)value, default(double)))
                .RegisterConverter(value => new Vector3d((double)value, default(double), default(double)))
                .RegisterConverter(value => new Vector4d((double)value, default(double), default(double), default(double)))
                .RegisterConverter(value => new Vector2i((int)value, default(int)))
                .RegisterConverter(value => new Vector3i((int)value, default(int), default(int)))
                .RegisterConverter(value => new Vector4i((int)value, default(int), default(int), default(int)))
                .RegisterConverter(value => new Vector4us((ushort)value, default(ushort), default(ushort), default(ushort)))
                .RegisterConverter(value => new Vector4ub((byte)value, default(byte), default(byte), default(byte)))
                .RegisterConverter(value => new Vector2l((long)value, default(long)))
                .RegisterConverter(value => new Vector3l((long)value, default(long), default(long)))
                .RegisterConverter(value => new Vector4l((long)value, default(long), default(long), default(long)))
                .RegisterConverter(value => new Vector2b(value!=default(float), default(bool)))
                .RegisterConverter(value => new Vector3b(value!=default(float), default(bool), default(bool)))
                .RegisterConverter(value => new Vector4b(value!=default(float), default(bool), default(bool), default(bool)))
                ;
            Default.RegisterType<Vector2>(new StreamMetaInfo(typeof(float), typeof(Vector2), 2, 1))
                .RegisterConverter(value => new Vector3(value.X, value.Y, default(float)))
                .RegisterConverter(value => new Vector4(value.X, value.Y, default(float), default(float)))
                .RegisterConverter(value => new Vector2d((double)value.X, (double)value.Y))
                .RegisterConverter(value => new Vector3d((double)value.X, (double)value.Y, default(double)))
                .RegisterConverter(value => new Vector4d((double)value.X, (double)value.Y, default(double), default(double)))
                .RegisterConverter(value => new Vector2i((int)value.X, (int)value.Y))
                .RegisterConverter(value => new Vector3i((int)value.X, (int)value.Y, default(int)))
                .RegisterConverter(value => new Vector4i((int)value.X, (int)value.Y, default(int), default(int)))
                .RegisterConverter(value => new Vector4us((ushort)value.X, (ushort)value.Y, default(ushort), default(ushort)))
                .RegisterConverter(value => new Vector4ub((byte)value.X, (byte)value.Y, default(byte), default(byte)))
                .RegisterConverter(value => new Vector2l((long)value.X, (long)value.Y))
                .RegisterConverter(value => new Vector3l((long)value.X, (long)value.Y, default(long)))
                .RegisterConverter(value => new Vector4l((long)value.X, (long)value.Y, default(long), default(long)))
                .RegisterConverter(value => new Vector2b(value.X!=default(float), value.Y!=default(float)))
                .RegisterConverter(value => new Vector3b(value.X!=default(float), value.Y!=default(float), default(bool)))
                .RegisterConverter(value => new Vector4b(value.X!=default(float), value.Y!=default(float), default(bool), default(bool)))
                ;
            Default.RegisterType<Vector3>(new StreamMetaInfo(typeof(float), typeof(Vector3), 3, 1))
                .RegisterConverter(value => new Vector2(value.X, value.Y))
                .RegisterConverter(value => new Vector4(value.X, value.Y, value.Z, default(float)))
                .RegisterConverter(value => new Vector2d((double)value.X, (double)value.Y))
                .RegisterConverter(value => new Vector3d((double)value.X, (double)value.Y, (double)value.Z))
                .RegisterConverter(value => new Vector4d((double)value.X, (double)value.Y, (double)value.Z, default(double)))
                .RegisterConverter(value => new Vector2i((int)value.X, (int)value.Y))
                .RegisterConverter(value => new Vector3i((int)value.X, (int)value.Y, (int)value.Z))
                .RegisterConverter(value => new Vector4i((int)value.X, (int)value.Y, (int)value.Z, default(int)))
                .RegisterConverter(value => new Vector4us((ushort)value.X, (ushort)value.Y, (ushort)value.Z, default(ushort)))
                .RegisterConverter(value => new Vector4ub((byte)value.X, (byte)value.Y, (byte)value.Z, default(byte)))
                .RegisterConverter(value => new Vector2l((long)value.X, (long)value.Y))
                .RegisterConverter(value => new Vector3l((long)value.X, (long)value.Y, (long)value.Z))
                .RegisterConverter(value => new Vector4l((long)value.X, (long)value.Y, (long)value.Z, default(long)))
                .RegisterConverter(value => new Vector2b(value.X!=default(float), value.Y!=default(float)))
                .RegisterConverter(value => new Vector3b(value.X!=default(float), value.Y!=default(float), value.Z!=default(float)))
                .RegisterConverter(value => new Vector4b(value.X!=default(float), value.Y!=default(float), value.Z!=default(float), default(bool)))
                ;
            Default.RegisterType<Vector4>(new StreamMetaInfo(typeof(float), typeof(Vector4), 4, 1))
                .RegisterConverter(value => new Vector2(value.X, value.Y))
                .RegisterConverter(value => new Vector3(value.X, value.Y, value.Z))
                .RegisterConverter(value => new Vector2d((double)value.X, (double)value.Y))
                .RegisterConverter(value => new Vector3d((double)value.X, (double)value.Y, (double)value.Z))
                .RegisterConverter(value => new Vector4d((double)value.X, (double)value.Y, (double)value.Z, (double)value.W))
                .RegisterConverter(value => new Vector2i((int)value.X, (int)value.Y))
                .RegisterConverter(value => new Vector3i((int)value.X, (int)value.Y, (int)value.Z))
                .RegisterConverter(value => new Vector4i((int)value.X, (int)value.Y, (int)value.Z, (int)value.W))
                .RegisterConverter(value => new Vector4us((ushort)value.X, (ushort)value.Y, (ushort)value.Z, (ushort)value.W))
                .RegisterConverter(value => new Vector4ub((byte)value.X, (byte)value.Y, (byte)value.Z, (byte)value.W))
                .RegisterConverter(value => new Vector2l((long)value.X, (long)value.Y))
                .RegisterConverter(value => new Vector3l((long)value.X, (long)value.Y, (long)value.Z))
                .RegisterConverter(value => new Vector4l((long)value.X, (long)value.Y, (long)value.Z, (long)value.W))
                .RegisterConverter(value => new Vector2b(value.X!=default(float), value.Y!=default(float)))
                .RegisterConverter(value => new Vector3b(value.X!=default(float), value.Y!=default(float), value.Z!=default(float)))
                .RegisterConverter(value => new Vector4b(value.X!=default(float), value.Y!=default(float), value.Z!=default(float), value.W!=default(float)))
                ;
            Default.RegisterType<Vector2d>(new StreamMetaInfo(typeof(double), typeof(Vector2d), 2, 1))
                .RegisterConverter(value => new Vector2((float)value.X, (float)value.Y))
                .RegisterConverter(value => new Vector3((float)value.X, (float)value.Y, default(float)))
                .RegisterConverter(value => new Vector4((float)value.X, (float)value.Y, default(float), default(float)))
                .RegisterConverter(value => new Vector3d(value.X, value.Y, default(double)))
                .RegisterConverter(value => new Vector4d(value.X, value.Y, default(double), default(double)))
                .RegisterConverter(value => new Vector2i((int)value.X, (int)value.Y))
                .RegisterConverter(value => new Vector3i((int)value.X, (int)value.Y, default(int)))
                .RegisterConverter(value => new Vector4i((int)value.X, (int)value.Y, default(int), default(int)))
                .RegisterConverter(value => new Vector4us((ushort)value.X, (ushort)value.Y, default(ushort), default(ushort)))
                .RegisterConverter(value => new Vector4ub((byte)value.X, (byte)value.Y, default(byte), default(byte)))
                .RegisterConverter(value => new Vector2l((long)value.X, (long)value.Y))
                .RegisterConverter(value => new Vector3l((long)value.X, (long)value.Y, default(long)))
                .RegisterConverter(value => new Vector4l((long)value.X, (long)value.Y, default(long), default(long)))
                .RegisterConverter(value => new Vector2b(value.X!=default(double), value.Y!=default(double)))
                .RegisterConverter(value => new Vector3b(value.X!=default(double), value.Y!=default(double), default(bool)))
                .RegisterConverter(value => new Vector4b(value.X!=default(double), value.Y!=default(double), default(bool), default(bool)))
                ;
            Default.RegisterType<Vector3d>(new StreamMetaInfo(typeof(double), typeof(Vector3d), 3, 1))
                .RegisterConverter(value => new Vector2((float)value.X, (float)value.Y))
                .RegisterConverter(value => new Vector3((float)value.X, (float)value.Y, (float)value.Z))
                .RegisterConverter(value => new Vector4((float)value.X, (float)value.Y, (float)value.Z, default(float)))
                .RegisterConverter(value => new Vector2d(value.X, value.Y))
                .RegisterConverter(value => new Vector4d(value.X, value.Y, value.Z, default(double)))
                .RegisterConverter(value => new Vector2i((int)value.X, (int)value.Y))
                .RegisterConverter(value => new Vector3i((int)value.X, (int)value.Y, (int)value.Z))
                .RegisterConverter(value => new Vector4i((int)value.X, (int)value.Y, (int)value.Z, default(int)))
                .RegisterConverter(value => new Vector4us((ushort)value.X, (ushort)value.Y, (ushort)value.Z, default(ushort)))
                .RegisterConverter(value => new Vector4ub((byte)value.X, (byte)value.Y, (byte)value.Z, default(byte)))
                .RegisterConverter(value => new Vector2l((long)value.X, (long)value.Y))
                .RegisterConverter(value => new Vector3l((long)value.X, (long)value.Y, (long)value.Z))
                .RegisterConverter(value => new Vector4l((long)value.X, (long)value.Y, (long)value.Z, default(long)))
                .RegisterConverter(value => new Vector2b(value.X!=default(double), value.Y!=default(double)))
                .RegisterConverter(value => new Vector3b(value.X!=default(double), value.Y!=default(double), value.Z!=default(double)))
                .RegisterConverter(value => new Vector4b(value.X!=default(double), value.Y!=default(double), value.Z!=default(double), default(bool)))
                ;
            Default.RegisterType<Vector4d>(new StreamMetaInfo(typeof(double), typeof(Vector4d), 4, 1))
                .RegisterConverter(value => new Vector2((float)value.X, (float)value.Y))
                .RegisterConverter(value => new Vector3((float)value.X, (float)value.Y, (float)value.Z))
                .RegisterConverter(value => new Vector4((float)value.X, (float)value.Y, (float)value.Z, (float)value.W))
                .RegisterConverter(value => new Vector2d(value.X, value.Y))
                .RegisterConverter(value => new Vector3d(value.X, value.Y, value.Z))
                .RegisterConverter(value => new Vector2i((int)value.X, (int)value.Y))
                .RegisterConverter(value => new Vector3i((int)value.X, (int)value.Y, (int)value.Z))
                .RegisterConverter(value => new Vector4i((int)value.X, (int)value.Y, (int)value.Z, (int)value.W))
                .RegisterConverter(value => new Vector4us((ushort)value.X, (ushort)value.Y, (ushort)value.Z, (ushort)value.W))
                .RegisterConverter(value => new Vector4ub((byte)value.X, (byte)value.Y, (byte)value.Z, (byte)value.W))
                .RegisterConverter(value => new Vector2l((long)value.X, (long)value.Y))
                .RegisterConverter(value => new Vector3l((long)value.X, (long)value.Y, (long)value.Z))
                .RegisterConverter(value => new Vector4l((long)value.X, (long)value.Y, (long)value.Z, (long)value.W))
                .RegisterConverter(value => new Vector2b(value.X!=default(double), value.Y!=default(double)))
                .RegisterConverter(value => new Vector3b(value.X!=default(double), value.Y!=default(double), value.Z!=default(double)))
                .RegisterConverter(value => new Vector4b(value.X!=default(double), value.Y!=default(double), value.Z!=default(double), value.W!=default(double)))
                ;
            Default.RegisterType<Vector2i>(new StreamMetaInfo(typeof(int), typeof(Vector2i), 2, 1))
                .RegisterConverter(value => new Vector2((float)value.X, (float)value.Y))
                .RegisterConverter(value => new Vector3((float)value.X, (float)value.Y, default(float)))
                .RegisterConverter(value => new Vector4((float)value.X, (float)value.Y, default(float), default(float)))
                .RegisterConverter(value => new Vector2d((double)value.X, (double)value.Y))
                .RegisterConverter(value => new Vector3d((double)value.X, (double)value.Y, default(double)))
                .RegisterConverter(value => new Vector4d((double)value.X, (double)value.Y, default(double), default(double)))
                .RegisterConverter(value => new Vector3i(value.X, value.Y, default(int)))
                .RegisterConverter(value => new Vector4i(value.X, value.Y, default(int), default(int)))
                .RegisterConverter(value => new Vector4us((ushort)value.X, (ushort)value.Y, default(ushort), default(ushort)))
                .RegisterConverter(value => new Vector4ub((byte)value.X, (byte)value.Y, default(byte), default(byte)))
                .RegisterConverter(value => new Vector2l((long)value.X, (long)value.Y))
                .RegisterConverter(value => new Vector3l((long)value.X, (long)value.Y, default(long)))
                .RegisterConverter(value => new Vector4l((long)value.X, (long)value.Y, default(long), default(long)))
                .RegisterConverter(value => new Vector2b(value.X!=default(int), value.Y!=default(int)))
                .RegisterConverter(value => new Vector3b(value.X!=default(int), value.Y!=default(int), default(bool)))
                .RegisterConverter(value => new Vector4b(value.X!=default(int), value.Y!=default(int), default(bool), default(bool)))
                ;
            Default.RegisterType<Vector3i>(new StreamMetaInfo(typeof(int), typeof(Vector3i), 3, 1))
                .RegisterConverter(value => new Vector2((float)value.X, (float)value.Y))
                .RegisterConverter(value => new Vector3((float)value.X, (float)value.Y, (float)value.Z))
                .RegisterConverter(value => new Vector4((float)value.X, (float)value.Y, (float)value.Z, default(float)))
                .RegisterConverter(value => new Vector2d((double)value.X, (double)value.Y))
                .RegisterConverter(value => new Vector3d((double)value.X, (double)value.Y, (double)value.Z))
                .RegisterConverter(value => new Vector4d((double)value.X, (double)value.Y, (double)value.Z, default(double)))
                .RegisterConverter(value => new Vector2i(value.X, value.Y))
                .RegisterConverter(value => new Vector4i(value.X, value.Y, value.Z, default(int)))
                .RegisterConverter(value => new Vector4us((ushort)value.X, (ushort)value.Y, (ushort)value.Z, default(ushort)))
                .RegisterConverter(value => new Vector4ub((byte)value.X, (byte)value.Y, (byte)value.Z, default(byte)))
                .RegisterConverter(value => new Vector2l((long)value.X, (long)value.Y))
                .RegisterConverter(value => new Vector3l((long)value.X, (long)value.Y, (long)value.Z))
                .RegisterConverter(value => new Vector4l((long)value.X, (long)value.Y, (long)value.Z, default(long)))
                .RegisterConverter(value => new Vector2b(value.X!=default(int), value.Y!=default(int)))
                .RegisterConverter(value => new Vector3b(value.X!=default(int), value.Y!=default(int), value.Z!=default(int)))
                .RegisterConverter(value => new Vector4b(value.X!=default(int), value.Y!=default(int), value.Z!=default(int), default(bool)))
                ;
            Default.RegisterType<Vector4i>(new StreamMetaInfo(typeof(int), typeof(Vector4i), 4, 1))
                .RegisterConverter(value => new Vector2((float)value.X, (float)value.Y))
                .RegisterConverter(value => new Vector3((float)value.X, (float)value.Y, (float)value.Z))
                .RegisterConverter(value => new Vector4((float)value.X, (float)value.Y, (float)value.Z, (float)value.W))
                .RegisterConverter(value => new Vector2d((double)value.X, (double)value.Y))
                .RegisterConverter(value => new Vector3d((double)value.X, (double)value.Y, (double)value.Z))
                .RegisterConverter(value => new Vector4d((double)value.X, (double)value.Y, (double)value.Z, (double)value.W))
                .RegisterConverter(value => new Vector2i(value.X, value.Y))
                .RegisterConverter(value => new Vector3i(value.X, value.Y, value.Z))
                .RegisterConverter(value => new Vector4us((ushort)value.X, (ushort)value.Y, (ushort)value.Z, (ushort)value.W))
                .RegisterConverter(value => new Vector4ub((byte)value.X, (byte)value.Y, (byte)value.Z, (byte)value.W))
                .RegisterConverter(value => new Vector2l((long)value.X, (long)value.Y))
                .RegisterConverter(value => new Vector3l((long)value.X, (long)value.Y, (long)value.Z))
                .RegisterConverter(value => new Vector4l((long)value.X, (long)value.Y, (long)value.Z, (long)value.W))
                .RegisterConverter(value => new Vector2b(value.X!=default(int), value.Y!=default(int)))
                .RegisterConverter(value => new Vector3b(value.X!=default(int), value.Y!=default(int), value.Z!=default(int)))
                .RegisterConverter(value => new Vector4b(value.X!=default(int), value.Y!=default(int), value.Z!=default(int), value.W!=default(int)))
                ;
            Default.RegisterType<Vector4us>(new StreamMetaInfo(typeof(ushort), typeof(Vector4us), 4, 1))
                .RegisterConverter(value => new Vector2((float)value.X, (float)value.Y))
                .RegisterConverter(value => new Vector3((float)value.X, (float)value.Y, (float)value.Z))
                .RegisterConverter(value => new Vector4((float)value.X, (float)value.Y, (float)value.Z, (float)value.W))
                .RegisterConverter(value => new Vector2d((double)value.X, (double)value.Y))
                .RegisterConverter(value => new Vector3d((double)value.X, (double)value.Y, (double)value.Z))
                .RegisterConverter(value => new Vector4d((double)value.X, (double)value.Y, (double)value.Z, (double)value.W))
                .RegisterConverter(value => new Vector2i((int)value.X, (int)value.Y))
                .RegisterConverter(value => new Vector3i((int)value.X, (int)value.Y, (int)value.Z))
                .RegisterConverter(value => new Vector4i((int)value.X, (int)value.Y, (int)value.Z, (int)value.W))
                .RegisterConverter(value => new Vector4ub((byte)value.X, (byte)value.Y, (byte)value.Z, (byte)value.W))
                .RegisterConverter(value => new Vector2l((long)value.X, (long)value.Y))
                .RegisterConverter(value => new Vector3l((long)value.X, (long)value.Y, (long)value.Z))
                .RegisterConverter(value => new Vector4l((long)value.X, (long)value.Y, (long)value.Z, (long)value.W))
                .RegisterConverter(value => new Vector2b(value.X!=default(ushort), value.Y!=default(ushort)))
                .RegisterConverter(value => new Vector3b(value.X!=default(ushort), value.Y!=default(ushort), value.Z!=default(ushort)))
                .RegisterConverter(value => new Vector4b(value.X!=default(ushort), value.Y!=default(ushort), value.Z!=default(ushort), value.W!=default(ushort)))
                ;
            Default.RegisterType<Vector4ub>(new StreamMetaInfo(typeof(byte), typeof(Vector4ub), 4, 1))
                .RegisterConverter(value => new Vector2((float)value.X, (float)value.Y))
                .RegisterConverter(value => new Vector3((float)value.X, (float)value.Y, (float)value.Z))
                .RegisterConverter(value => new Vector4((float)value.X, (float)value.Y, (float)value.Z, (float)value.W))
                .RegisterConverter(value => new Vector2d((double)value.X, (double)value.Y))
                .RegisterConverter(value => new Vector3d((double)value.X, (double)value.Y, (double)value.Z))
                .RegisterConverter(value => new Vector4d((double)value.X, (double)value.Y, (double)value.Z, (double)value.W))
                .RegisterConverter(value => new Vector2i((int)value.X, (int)value.Y))
                .RegisterConverter(value => new Vector3i((int)value.X, (int)value.Y, (int)value.Z))
                .RegisterConverter(value => new Vector4i((int)value.X, (int)value.Y, (int)value.Z, (int)value.W))
                .RegisterConverter(value => new Vector4us((ushort)value.X, (ushort)value.Y, (ushort)value.Z, (ushort)value.W))
                .RegisterConverter(value => new Vector2l((long)value.X, (long)value.Y))
                .RegisterConverter(value => new Vector3l((long)value.X, (long)value.Y, (long)value.Z))
                .RegisterConverter(value => new Vector4l((long)value.X, (long)value.Y, (long)value.Z, (long)value.W))
                .RegisterConverter(value => new Vector2b(value.X!=default(byte), value.Y!=default(byte)))
                .RegisterConverter(value => new Vector3b(value.X!=default(byte), value.Y!=default(byte), value.Z!=default(byte)))
                .RegisterConverter(value => new Vector4b(value.X!=default(byte), value.Y!=default(byte), value.Z!=default(byte), value.W!=default(byte)))
                ;
            Default.RegisterType<Vector2l>(new StreamMetaInfo(typeof(long), typeof(Vector2l), 2, 1))
                .RegisterConverter(value => new Vector2((float)value.X, (float)value.Y))
                .RegisterConverter(value => new Vector3((float)value.X, (float)value.Y, default(float)))
                .RegisterConverter(value => new Vector4((float)value.X, (float)value.Y, default(float), default(float)))
                .RegisterConverter(value => new Vector2d((double)value.X, (double)value.Y))
                .RegisterConverter(value => new Vector3d((double)value.X, (double)value.Y, default(double)))
                .RegisterConverter(value => new Vector4d((double)value.X, (double)value.Y, default(double), default(double)))
                .RegisterConverter(value => new Vector2i((int)value.X, (int)value.Y))
                .RegisterConverter(value => new Vector3i((int)value.X, (int)value.Y, default(int)))
                .RegisterConverter(value => new Vector4i((int)value.X, (int)value.Y, default(int), default(int)))
                .RegisterConverter(value => new Vector4us((ushort)value.X, (ushort)value.Y, default(ushort), default(ushort)))
                .RegisterConverter(value => new Vector4ub((byte)value.X, (byte)value.Y, default(byte), default(byte)))
                .RegisterConverter(value => new Vector3l(value.X, value.Y, default(long)))
                .RegisterConverter(value => new Vector4l(value.X, value.Y, default(long), default(long)))
                .RegisterConverter(value => new Vector2b(value.X!=default(long), value.Y!=default(long)))
                .RegisterConverter(value => new Vector3b(value.X!=default(long), value.Y!=default(long), default(bool)))
                .RegisterConverter(value => new Vector4b(value.X!=default(long), value.Y!=default(long), default(bool), default(bool)))
                ;
            Default.RegisterType<Vector3l>(new StreamMetaInfo(typeof(long), typeof(Vector3l), 3, 1))
                .RegisterConverter(value => new Vector2((float)value.X, (float)value.Y))
                .RegisterConverter(value => new Vector3((float)value.X, (float)value.Y, (float)value.Z))
                .RegisterConverter(value => new Vector4((float)value.X, (float)value.Y, (float)value.Z, default(float)))
                .RegisterConverter(value => new Vector2d((double)value.X, (double)value.Y))
                .RegisterConverter(value => new Vector3d((double)value.X, (double)value.Y, (double)value.Z))
                .RegisterConverter(value => new Vector4d((double)value.X, (double)value.Y, (double)value.Z, default(double)))
                .RegisterConverter(value => new Vector2i((int)value.X, (int)value.Y))
                .RegisterConverter(value => new Vector3i((int)value.X, (int)value.Y, (int)value.Z))
                .RegisterConverter(value => new Vector4i((int)value.X, (int)value.Y, (int)value.Z, default(int)))
                .RegisterConverter(value => new Vector4us((ushort)value.X, (ushort)value.Y, (ushort)value.Z, default(ushort)))
                .RegisterConverter(value => new Vector4ub((byte)value.X, (byte)value.Y, (byte)value.Z, default(byte)))
                .RegisterConverter(value => new Vector2l(value.X, value.Y))
                .RegisterConverter(value => new Vector4l(value.X, value.Y, value.Z, default(long)))
                .RegisterConverter(value => new Vector2b(value.X!=default(long), value.Y!=default(long)))
                .RegisterConverter(value => new Vector3b(value.X!=default(long), value.Y!=default(long), value.Z!=default(long)))
                .RegisterConverter(value => new Vector4b(value.X!=default(long), value.Y!=default(long), value.Z!=default(long), default(bool)))
                ;
            Default.RegisterType<Vector4l>(new StreamMetaInfo(typeof(long), typeof(Vector4l), 4, 1))
                .RegisterConverter(value => new Vector2((float)value.X, (float)value.Y))
                .RegisterConverter(value => new Vector3((float)value.X, (float)value.Y, (float)value.Z))
                .RegisterConverter(value => new Vector4((float)value.X, (float)value.Y, (float)value.Z, (float)value.W))
                .RegisterConverter(value => new Vector2d((double)value.X, (double)value.Y))
                .RegisterConverter(value => new Vector3d((double)value.X, (double)value.Y, (double)value.Z))
                .RegisterConverter(value => new Vector4d((double)value.X, (double)value.Y, (double)value.Z, (double)value.W))
                .RegisterConverter(value => new Vector2i((int)value.X, (int)value.Y))
                .RegisterConverter(value => new Vector3i((int)value.X, (int)value.Y, (int)value.Z))
                .RegisterConverter(value => new Vector4i((int)value.X, (int)value.Y, (int)value.Z, (int)value.W))
                .RegisterConverter(value => new Vector4us((ushort)value.X, (ushort)value.Y, (ushort)value.Z, (ushort)value.W))
                .RegisterConverter(value => new Vector4ub((byte)value.X, (byte)value.Y, (byte)value.Z, (byte)value.W))
                .RegisterConverter(value => new Vector2l(value.X, value.Y))
                .RegisterConverter(value => new Vector3l(value.X, value.Y, value.Z))
                .RegisterConverter(value => new Vector2b(value.X!=default(long), value.Y!=default(long)))
                .RegisterConverter(value => new Vector3b(value.X!=default(long), value.Y!=default(long), value.Z!=default(long)))
                .RegisterConverter(value => new Vector4b(value.X!=default(long), value.Y!=default(long), value.Z!=default(long), value.W!=default(long)))
                ;
            Default.RegisterType<Vector2b>(new StreamMetaInfo(typeof(bool), typeof(Vector2b), 2, 1))
                .RegisterConverter(value => new Vector2(value.X?(float)1:default(float), value.Y?(float)1:default(float)))
                .RegisterConverter(value => new Vector3(value.X?(float)1:default(float), value.Y?(float)1:default(float), default(float)))
                .RegisterConverter(value => new Vector4(value.X?(float)1:default(float), value.Y?(float)1:default(float), default(float), default(float)))
                .RegisterConverter(value => new Vector2d(value.X?(double)1:default(double), value.Y?(double)1:default(double)))
                .RegisterConverter(value => new Vector3d(value.X?(double)1:default(double), value.Y?(double)1:default(double), default(double)))
                .RegisterConverter(value => new Vector4d(value.X?(double)1:default(double), value.Y?(double)1:default(double), default(double), default(double)))
                .RegisterConverter(value => new Vector2i(value.X?(int)1:default(int), value.Y?(int)1:default(int)))
                .RegisterConverter(value => new Vector3i(value.X?(int)1:default(int), value.Y?(int)1:default(int), default(int)))
                .RegisterConverter(value => new Vector4i(value.X?(int)1:default(int), value.Y?(int)1:default(int), default(int), default(int)))
                .RegisterConverter(value => new Vector4us(value.X?(ushort)1:default(ushort), value.Y?(ushort)1:default(ushort), default(ushort), default(ushort)))
                .RegisterConverter(value => new Vector4ub(value.X?(byte)1:default(byte), value.Y?(byte)1:default(byte), default(byte), default(byte)))
                .RegisterConverter(value => new Vector2l(value.X?(long)1:default(long), value.Y?(long)1:default(long)))
                .RegisterConverter(value => new Vector3l(value.X?(long)1:default(long), value.Y?(long)1:default(long), default(long)))
                .RegisterConverter(value => new Vector4l(value.X?(long)1:default(long), value.Y?(long)1:default(long), default(long), default(long)))
                .RegisterConverter(value => new Vector3b(value.X, value.Y, default(bool)))
                .RegisterConverter(value => new Vector4b(value.X, value.Y, default(bool), default(bool)))
                ;
            Default.RegisterType<Vector3b>(new StreamMetaInfo(typeof(bool), typeof(Vector3b), 3, 1))
                .RegisterConverter(value => new Vector2(value.X?(float)1:default(float), value.Y?(float)1:default(float)))
                .RegisterConverter(value => new Vector3(value.X?(float)1:default(float), value.Y?(float)1:default(float), value.Z?(float)1:default(float)))
                .RegisterConverter(value => new Vector4(value.X?(float)1:default(float), value.Y?(float)1:default(float), value.Z?(float)1:default(float), default(float)))
                .RegisterConverter(value => new Vector2d(value.X?(double)1:default(double), value.Y?(double)1:default(double)))
                .RegisterConverter(value => new Vector3d(value.X?(double)1:default(double), value.Y?(double)1:default(double), value.Z?(double)1:default(double)))
                .RegisterConverter(value => new Vector4d(value.X?(double)1:default(double), value.Y?(double)1:default(double), value.Z?(double)1:default(double), default(double)))
                .RegisterConverter(value => new Vector2i(value.X?(int)1:default(int), value.Y?(int)1:default(int)))
                .RegisterConverter(value => new Vector3i(value.X?(int)1:default(int), value.Y?(int)1:default(int), value.Z?(int)1:default(int)))
                .RegisterConverter(value => new Vector4i(value.X?(int)1:default(int), value.Y?(int)1:default(int), value.Z?(int)1:default(int), default(int)))
                .RegisterConverter(value => new Vector4us(value.X?(ushort)1:default(ushort), value.Y?(ushort)1:default(ushort), value.Z?(ushort)1:default(ushort), default(ushort)))
                .RegisterConverter(value => new Vector4ub(value.X?(byte)1:default(byte), value.Y?(byte)1:default(byte), value.Z?(byte)1:default(byte), default(byte)))
                .RegisterConverter(value => new Vector2l(value.X?(long)1:default(long), value.Y?(long)1:default(long)))
                .RegisterConverter(value => new Vector3l(value.X?(long)1:default(long), value.Y?(long)1:default(long), value.Z?(long)1:default(long)))
                .RegisterConverter(value => new Vector4l(value.X?(long)1:default(long), value.Y?(long)1:default(long), value.Z?(long)1:default(long), default(long)))
                .RegisterConverter(value => new Vector2b(value.X, value.Y))
                .RegisterConverter(value => new Vector4b(value.X, value.Y, value.Z, default(bool)))
                ;
            Default.RegisterType<Vector4b>(new StreamMetaInfo(typeof(bool), typeof(Vector4b), 4, 1))
                .RegisterConverter(value => new Vector2(value.X?(float)1:default(float), value.Y?(float)1:default(float)))
                .RegisterConverter(value => new Vector3(value.X?(float)1:default(float), value.Y?(float)1:default(float), value.Z?(float)1:default(float)))
                .RegisterConverter(value => new Vector4(value.X?(float)1:default(float), value.Y?(float)1:default(float), value.Z?(float)1:default(float), value.W?(float)1:default(float)))
                .RegisterConverter(value => new Vector2d(value.X?(double)1:default(double), value.Y?(double)1:default(double)))
                .RegisterConverter(value => new Vector3d(value.X?(double)1:default(double), value.Y?(double)1:default(double), value.Z?(double)1:default(double)))
                .RegisterConverter(value => new Vector4d(value.X?(double)1:default(double), value.Y?(double)1:default(double), value.Z?(double)1:default(double), value.W?(double)1:default(double)))
                .RegisterConverter(value => new Vector2i(value.X?(int)1:default(int), value.Y?(int)1:default(int)))
                .RegisterConverter(value => new Vector3i(value.X?(int)1:default(int), value.Y?(int)1:default(int), value.Z?(int)1:default(int)))
                .RegisterConverter(value => new Vector4i(value.X?(int)1:default(int), value.Y?(int)1:default(int), value.Z?(int)1:default(int), value.W?(int)1:default(int)))
                .RegisterConverter(value => new Vector4us(value.X?(ushort)1:default(ushort), value.Y?(ushort)1:default(ushort), value.Z?(ushort)1:default(ushort), value.W?(ushort)1:default(ushort)))
                .RegisterConverter(value => new Vector4ub(value.X?(byte)1:default(byte), value.Y?(byte)1:default(byte), value.Z?(byte)1:default(byte), value.W?(byte)1:default(byte)))
                .RegisterConverter(value => new Vector2l(value.X?(long)1:default(long), value.Y?(long)1:default(long)))
                .RegisterConverter(value => new Vector3l(value.X?(long)1:default(long), value.Y?(long)1:default(long), value.Z?(long)1:default(long)))
                .RegisterConverter(value => new Vector4l(value.X?(long)1:default(long), value.Y?(long)1:default(long), value.Z?(long)1:default(long), value.W?(long)1:default(long)))
                .RegisterConverter(value => new Vector2b(value.X, value.Y))
                .RegisterConverter(value => new Vector3b(value.X, value.Y, value.Z))
                ;

        }
    }
}