using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Votacao.Extensions
{
    public static class WebHostBuilderExtensions
    {
        internal static List<Assembly> Assemblies { get; private set; } = new List<Assembly>();

        public static IWebHostBuilder LoadAllDllsBinFolder(this IWebHostBuilder builder)
        {
            string binPath = AppDomain.CurrentDomain.BaseDirectory;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.GetName().Name.StartsWith("Votacao"));
            Assemblies.AddRange(assemblies);

            foreach (string dll in Directory.GetFiles(binPath, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    if (!assemblies.Select(x => x.GetName().Name).Contains(Path.GetFileNameWithoutExtension(dll)) &&
                        Path.GetFileNameWithoutExtension(dll).StartsWith("Aprese"))
                    {
                        var assembly = Assembly.LoadFile(dll);
                        Assemblies.Add(assembly);
                    }
                }
                catch (FileLoadException) { }
                catch (BadImageFormatException) { }
            }

            return builder;
        }
    }
}
