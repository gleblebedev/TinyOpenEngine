﻿using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Toe.SceneGraph
{
    public class LocalTransform
    {
        private static readonly TransformUpdatedArgs _transformUpdatedArgs = new TransformUpdatedArgs();
        private Flags _flags = Flags.Valid;
        private Matrix4x4 _matrix = Matrix4x4.Identity;
        private Vector3 _position = Vector3.Zero;
        private Quaternion _rotation = Quaternion.Identity;
        private Vector3 _scale = Vector3.One;

        public Matrix4x4 Matrix
        {
            get
            {
                UpdateMatrixIfNeeded();
                return _matrix;
            }
            set
            {
                UpdateMatrixIfNeeded();
                InvalidateFlag(Flags.InvalidPRS);
                _matrix = value;
            }
        }

        public Vector3 Scale
        {
            get
            {
                UpdatePRSIfNeeded();
                return _scale;
            }
            set
            {
                UpdatePRSIfNeeded();
                if (_scale == value) return;
                InvalidateFlag(Flags.InvalidMatrix);
                _scale = value;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                UpdatePRSIfNeeded();
                return _rotation;
            }
            set
            {
                UpdatePRSIfNeeded();
                if (_rotation == value) return;

                InvalidateFlag(Flags.InvalidMatrix);
                _rotation = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                UpdatePRSIfNeeded();
                return _position;
            }
            set
            {
                UpdatePRSIfNeeded();
                if (_position == value) return;

                InvalidateFlag(Flags.InvalidMatrix);
                _position = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InvalidateFlag(Flags flag)
        {
            _flags |= flag;
            OnUpdate?.Invoke(this, _transformUpdatedArgs);
        }

        public event EventHandler<TransformUpdatedArgs> OnUpdate;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateMatrixIfNeeded()
        {
            if (Flags.InvalidMatrix == (_flags & Flags.InvalidMatrix))
            {
                _flags &= ~Flags.InvalidMatrix;
                _matrix = Matrix4x4.CreateScale(_scale) * Matrix4x4.CreateFromQuaternion(_rotation) *
                          Matrix4x4.CreateTranslation(_position);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdatePRSIfNeeded()
        {
            if (Flags.InvalidPRS == (_flags & Flags.InvalidPRS))
            {
                _flags &= ~Flags.InvalidPRS;
                Matrix4x4.Decompose(_matrix, out _scale, out _rotation, out _position);
            }
        }

        public void Reset()
        {
            _matrix = Matrix4x4.Identity;
            _position = Vector3.Zero;
            _rotation = Quaternion.Identity;
            _scale = Vector3.One;
            _flags = Flags.Valid;
        }

        [Flags]
        private enum Flags
        {
            Valid = 0,
            InvalidMatrix = 1,
            InvalidPRS = 2
        }

        public void CopyFrom(LocalTransform other)
        {
            _flags = other._flags;
            if (0 == (_flags & Flags.InvalidMatrix))
            {
                _matrix = other._matrix;
            }
            if (0 == (_flags & Flags.InvalidPRS))
            {
                _position = other._position;
                _rotation = other._rotation;
                _scale = other._scale;
            }
        }
    }
}