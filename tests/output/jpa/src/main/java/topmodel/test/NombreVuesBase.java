package topmodel.test;

import jakarta.persistence.Transient;

public class NombreVuesBase {

    /**
     * Nombre de vues de l'avis (calculé).
     */
    @Transient
    private Integer nombreVues = 0;

    /**
     * Getter for nombreVues.
     *
     * @return value of {@link #nombreVues nombreVues}.
     */
    public Integer getNombreVues() {
        return this.nombreVues;
    }

    /**
     * Set the value of {@link #nombreVues nombreVues}.
     * 
     * @param nombreVues value to set.
     */
    public void setNombreVues(Integer nombreVues) {
        this.nombreVues = nombreVues;
    }
}
