namespace TopModel.Core.Model;

public enum AssociationType
{
    /// <summary>
    /// Many to one.
    /// </summary>
    ManyToOne,

    /// <summary>
    /// One to one.
    /// </summary>
    OneToOne,

    /// <summary>
    /// One to many.
    /// </summary>
    OneToMany,

    /// <summary>
    /// Many to many.
    /// </summary>
    ManyToMany
}