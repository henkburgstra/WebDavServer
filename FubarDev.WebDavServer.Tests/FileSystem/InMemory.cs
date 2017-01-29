﻿using System.IO;
using System.Threading;
using System.Threading.Tasks;

using FubarDev.WebDavServer.FileSystem;
using FubarDev.WebDavServer.FileSystem.InMemory;
using FubarDev.WebDavServer.Properties.Store.InMemory;

using Xunit;

namespace FubarDev.WebDavServer.Tests.FileSystem
{
    public class InMemory
    {
        [Fact]
        public async Task Empty()
        {
            var ct = CancellationToken.None;
            var fs = new InMemoryFileSystem(new PathTraversalEngine(), new InMemoryPropertyStoreFactory());
            var root = await fs.Root;
            var rootChildren = await root.GetChildrenAsync(ct).ConfigureAwait(false);
            Assert.Equal(0, rootChildren.Count);
        }

        [Fact]
        public async Task SingleEmptyDirectory()
        {
            var ct = CancellationToken.None;
            var fs = new InMemoryFileSystem(new PathTraversalEngine(), new InMemoryPropertyStoreFactory());
            var root = await fs.Root;
            var test1 = await root.CreateCollectionAsync("test1", ct).ConfigureAwait(false);
            var rootChildren = await root.GetChildrenAsync(ct).ConfigureAwait(false);
            Assert.Collection(
                rootChildren,
                child =>
                {
                    Assert.NotNull(child);
                    var coll = Assert.IsAssignableFrom<ICollection>(child);
                    Assert.Same(test1, coll);
                    Assert.Equal("test1", coll.Name);
                    Assert.Same(root, child.Parent);
                });
        }

        [Fact]
        public async Task TwoEmptyDirectories()
        {
            var ct = CancellationToken.None;
            var fs = new InMemoryFileSystem(new PathTraversalEngine(), new InMemoryPropertyStoreFactory());
            var root = await fs.Root;
            var test1 = await root.CreateCollectionAsync("test1", ct).ConfigureAwait(false);
            var test2 = await root.CreateCollectionAsync("test2", ct).ConfigureAwait(false);
            var rootChildren = await root.GetChildrenAsync(ct).ConfigureAwait(false);
            Assert.Collection(
                rootChildren,
                child =>
                {
                    Assert.NotNull(child);
                    var coll = Assert.IsAssignableFrom<ICollection>(child);
                    Assert.Same(test1, coll);
                    Assert.Equal("test1", coll.Name);
                    Assert.Same(root, child.Parent);
                },
                child =>
                {
                    Assert.NotNull(child);
                    var coll = Assert.IsAssignableFrom<ICollection>(child);
                    Assert.Same(test2, coll);
                    Assert.Equal("test2", coll.Name);
                    Assert.Same(root, child.Parent);
                });
        }

        [Fact]
        public async Task CannotAddTwoDirectoriesWithSameName()
        {
            var ct = CancellationToken.None;
            var fs = new InMemoryFileSystem(new PathTraversalEngine(), new InMemoryPropertyStoreFactory());
            var root = await fs.Root;
            await root.CreateCollectionAsync("test1", ct).ConfigureAwait(false);
            await Assert.ThrowsAnyAsync<IOException>(async () => await root.CreateCollectionAsync("test1", ct).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}