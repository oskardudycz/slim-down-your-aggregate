package io.eventdriven.slimdownaggregates.original.persistence.books;

import io.eventdriven.slimdownaggregates.original.persistence.authors.AuthorEntity;
import io.eventdriven.slimdownaggregates.original.persistence.books.entities.ChapterEntity;
import io.eventdriven.slimdownaggregates.original.persistence.books.entities.FormatEntity;
import io.eventdriven.slimdownaggregates.original.persistence.books.valueobjects.CommitteeApprovalVO;
import io.eventdriven.slimdownaggregates.original.persistence.books.valueobjects.TranslationVO;
import io.eventdriven.slimdownaggregates.original.persistence.publishers.PublisherEntity;
import io.eventdriven.slimdownaggregates.original.persistence.reviewers.ReviewerEntity;
import jakarta.persistence.*;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@Entity
@Table(name = "books")
public class BookEntity {

  public enum State { Writing, Editing, Printing, Published, OutOfPrint }

  @Id
  @GeneratedValue(strategy = GenerationType.AUTO)
  private UUID id;

  @Enumerated(EnumType.STRING)
  private State currentState;

  private String title;

  @ManyToOne
  @JoinColumn(name = "author_id")
  private AuthorEntity author;

  @ManyToOne
  @JoinColumn(name = "publisher_id")
  private PublisherEntity publisher;

  private int edition;
  private String genre;
  private String isbn;
  private LocalDate publicationDate;
  private Integer totalPages;
  private Integer numberOfIllustrations;
  private String bindingType;

  @Lob
  private String summary;

  @Embedded
  private CommitteeApprovalVO committeeApproval;

  @OneToMany(mappedBy = "book", cascade = CascadeType.ALL, orphanRemoval = true)
  private List<ReviewerEntity> reviewers = new ArrayList<>();

  @OneToMany(mappedBy = "book", cascade = CascadeType.ALL, orphanRemoval = true)
  private List<ChapterEntity> chapters = new ArrayList<>();

  @ElementCollection
  @CollectionTable(name = "book_translations", joinColumns = @JoinColumn(name = "book_id"))
  private List<TranslationVO> translations = new ArrayList<>();

  @OneToMany(mappedBy = "book", cascade = CascadeType.ALL, orphanRemoval = true)
  private List<FormatEntity> formats = new ArrayList<>();

  @Version
  private int version;

  public BookEntity() {}

  // All getters and setters

  public UUID getId() { return id; }
  public void setId(UUID id) { this.id = id; }
  public State getCurrentState() { return currentState; }
  public void setCurrentState(State currentState) { this.currentState = currentState; }
  public String getTitle() { return title; }
  public void setTitle(String title) { this.title = title; }
  public AuthorEntity getAuthor() { return author; }
  public void setAuthor(AuthorEntity author) { this.author = author; }
  public PublisherEntity getPublisher() { return publisher; }
  public void setPublisher(PublisherEntity publisher) { this.publisher = publisher; }
  public int getEdition() { return edition; }
  public void setEdition(int edition) { this.edition = edition; }
  public String getGenre() { return genre; }
  public void setGenre(String genre) { this.genre = genre; }
  public String getIsbn() { return isbn; }
  public void setIsbn(String isbn) { this.isbn = isbn; }
  public LocalDate getPublicationDate() { return publicationDate; }
  public void setPublicationDate(LocalDate publicationDate) { this.publicationDate = publicationDate; }
  public Integer getTotalPages() { return totalPages; }
  public void setTotalPages(Integer totalPages) { this.totalPages = totalPages; }
  public Integer getNumberOfIllustrations() { return numberOfIllustrations; }
  public void setNumberOfIllustrations(Integer numberOfIllustrations) { this.numberOfIllustrations = numberOfIllustrations; }
  public String getBindingType() { return bindingType; }
  public void setBindingType(String bindingType) { this.bindingType = bindingType; }
  public String getSummary() { return summary; }
  public void setSummary(String summary) { this.summary = summary; }
  public CommitteeApprovalVO getCommitteeApproval() { return committeeApproval; }
  public void setCommitteeApproval(CommitteeApprovalVO committeeApproval) { this.committeeApproval = committeeApproval; }
  public List<ReviewerEntity> getReviewers() { return reviewers; }
  public void setReviewers(List<ReviewerEntity> reviewers) { this.reviewers = reviewers; }
  public List<ChapterEntity> getChapters() { return chapters; }
  public void setChapters(List<ChapterEntity> chapters) { this.chapters = chapters; }
  public List<TranslationVO> getTranslations() { return translations; }
  public void setTranslations(List<TranslationVO> translations) { this.translations = translations; }
  public List<FormatEntity> getFormats() { return formats; }
  public void setFormats(List<FormatEntity> formats) { this.formats = formats; }
  public int getVersion() { return version; }
  public void setVersion(int version) { this.version = version; }
}

