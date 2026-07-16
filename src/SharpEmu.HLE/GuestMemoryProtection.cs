// Copyright (C) 2026 SharpEmu Emulator Project
// SPDX-License-Identifier: GPL-2.0-or-later

using System.Runtime.InteropServices;

namespace SharpEmu.HLE;

/// <summary>
/// Lightweight guest-memory protection helpers that expose a safe span view over
/// a raw guest buffer while enforcing the requested protection levels.
/// </summary>
public static unsafe class GuestMemoryProtection
{
    public static bool TryCreateSpan(
        byte* buffer,
        int length,
        GuestPageProtection protection,
        out Span<byte> span)
    {
        span = default;
        if (buffer is null || length < 0)
        {
            return false;
        }

        if ((protection & GuestPageProtection.Read) == 0 && (protection & GuestPageProtection.Execute) == 0)
        {
            return false;
        }

        if ((protection & GuestPageProtection.Execute) != 0 && (protection & GuestPageProtection.Read) == 0)
        {
            return false;
        }

        if (length == 0)
        {
            span = Span<byte>.Empty;
            return true;
        }

        span = new Span<byte>(buffer, length);
        return true;
    }

    public static bool TryCreateReadOnlySpan(byte* buffer, int length, out ReadOnlySpan<byte> span)
    {
        span = default;
        if (buffer is null || length < 0)
        {
            return false;
        }

        if (length == 0)
        {
            span = ReadOnlySpan<byte>.Empty;
            return true;
        }

        span = new ReadOnlySpan<byte>(buffer, length);
        return true;
    }

    public static bool TryCreateReadWriteSpan(byte* buffer, int length, out Span<byte> span)
        => TryCreateSpan(buffer, length, GuestPageProtection.Read | GuestPageProtection.Write, out span);
}
