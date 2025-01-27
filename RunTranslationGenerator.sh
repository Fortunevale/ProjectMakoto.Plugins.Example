cd Tools/TranslationSourceGenerator

git submodule update --init --depth 0

dotnet restore
dotnet run -- ../../Translations/strings.json ../../Entities/Translations.cs ProjectMakoto.Plugins.Example.Entities
sleep 60