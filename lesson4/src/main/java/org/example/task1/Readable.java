package org.example.task1;

import java.io.File;
import java.util.Collection;

public interface Readable {
    Collection<String> readTextFile(File file) throws RuntimeException;
}
