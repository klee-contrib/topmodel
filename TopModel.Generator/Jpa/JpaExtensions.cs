namespace TopModel.Generator.Jpa
{
    public static class JpaExtensions
    {
        public static string GetAssociationName(this AssociationProperty ap)
        {
            return $"{ap.Association.Name.ToFirstLower()}{ap.Role?.ToFirstUpper() ?? string.Empty}{(ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany ? "List" : string.Empty)}";
        }
    }
}
