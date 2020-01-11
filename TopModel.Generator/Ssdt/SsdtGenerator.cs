using TopModel.Core;
using TopModel.Core.Config;

namespace TopModel.Generator.Ssdt
{
    public class SsdtGenerator : IGenerator
    {
        private readonly SsdtConfig? _config;
        private readonly ModelStore _modelStore;

        public SsdtGenerator(ModelStore modelStore, SsdtConfig? config = null)
        {
            _config = config;
            _modelStore = modelStore;
        }

        public void Generate()
        {
            if (_config == null)
            {
                return;
            }

            if (_config.TableScriptFolder != null && _config.TableTypeScriptFolder != null)
            {
                // Génération pour déploiement SSDT.
                new SqlServerSsdtSchemaGenerator().GenerateSchemaScript(
                    _modelStore.Classes,
                    _config.TableScriptFolder,
                    _config.TableTypeScriptFolder);
            }

            var ssdtInsertGenerator = new SqlServerSsdtInsertGenerator(_config);

            if (_config.InitStaticListMainScriptName != null && _config.InitStaticListScriptFolder != null)
            {
                ssdtInsertGenerator.GenerateListInitScript(
                    _modelStore.StaticListsMap,
                    _config.InitStaticListScriptFolder,
                    _config.InitStaticListMainScriptName,
                    "delta_static_lists.sql",
                    true);
            }

            if ( _config.InitReferenceListMainScriptName != null && _config.InitReferenceListScriptFolder != null)
            {
                ssdtInsertGenerator.GenerateListInitScript(
                    _modelStore.ReferenceListsMap,
                    _config.InitReferenceListScriptFolder,
                    _config.InitReferenceListMainScriptName,
                    "delta_reference_lists.sql",
                    false);
            }
        }
    }
}
