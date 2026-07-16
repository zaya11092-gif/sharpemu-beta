// Copyright (C) 2026 SharpEmu Emulator Project
// SPDX-License-Identifier: GPL-2.0-or-later

using SharpEmu.HLE;
using SharpEmu.Libs.Kernel;
using Xunit;

namespace SharpEmu.Libs.Tests.Kernel;

public sealed class KernelHlePrimitivesTests
{
    [Fact]
    public unsafe void TryCreateSpan_AllowsReadAndWrite_WhenProtectionMatches()
    {
        byte* buffer = stackalloc byte[16];
        buffer[0] = 0x5A;
        buffer[1] = 0xA5;

        Assert.True(GuestMemoryProtection.TryCreateSpan(
            buffer,
            2,
            GuestPageProtection.Read | GuestPageProtection.Write,
            out var span));

        Assert.Equal(0x5A, span[0]);
        Assert.Equal(0xA5, span[1]);
    }

    [Fact]
    public unsafe void TryCreateSpan_RejectsExecuteWithoutRead()
    {
        byte* buffer = stackalloc byte[16];

        Assert.False(GuestMemoryProtection.TryCreateSpan(
            buffer,
            16,
            GuestPageProtection.Execute,
            out _));
    }
}
