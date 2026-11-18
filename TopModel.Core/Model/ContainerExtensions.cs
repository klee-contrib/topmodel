namespace TopModel.Core.Model;

public static class ContainerExtensions
{
    extension(Class classe)
    {
        /// <summary>
        /// Association vers la classe parente pour une classe dérivée.
        /// </summary>
        public IProperty? ParentAssociationProperty =>
            classe.Extends != null && classe.Extends.InheritanceStrategy == InheritanceStrategy.JoinedTables
                ? new AssociationProperty(null!)
                {
                    Association = classe.Extends,
                    Class = classe,
                    Comment = "Association vers la clé primaire de la classe parente",
                    Required = true,
                    PrimaryKey = !classe.PrimaryKey.Any(),
                }
                : null;

        /// <summary>
        /// Récupère le potentiel mapper de la classe courante vers la classe cible.
        /// </summary>
        /// <param name="targetClass">Classe cible.</param>
        /// <returns>Mapper (from ou to), s'il existe.</returns>
        public Mapper? GetMapperTo(Class targetClass)
        {
            var fromMapper = targetClass.FromMappers.FirstOrDefault(fm =>
                fm.Params.Count == 1 && fm.ClassParams.SingleOrDefault()?.Class == classe
            );
            if (fromMapper != null)
            {
                return fromMapper;
            }

            var toMapper = classe.ToMappers.FirstOrDefault(tm => tm.Class == targetClass);
            if (toMapper != null)
            {
                return toMapper;
            }

            return null;
        }
    }

    extension(IPropertyContainer container)
    {
        internal IEnumerable<Decorator> AllDecorators
        {
            get
            {
                if (container is Decorator self)
                {
                    yield return self;
                }

                foreach (var (d, _) in container.Decorators)
                {
                    yield return d;

                    foreach (var subD in d.AllDecorators)
                    {
                        yield return subD;
                    }
                }
            }
        }

        internal IList<IProperty> OwnProperties =>
            container switch
            {
                Class c => c.OwnProperties,
                Endpoint e => e.OwnParams.Concat([e.OwnReturns!]).Where(p => p != null).ToList(),
                Decorator d => d.OwnProperties,
                PropertyMapping p => [p.Property],
                _ => throw new InvalidOperationException(),
            };
    }
}
