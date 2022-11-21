//
// Created by LXH on 2021/7/20.
//

#ifndef IRIS_OBSERVER_MANAGER_H_
#define IRIS_OBSERVER_MANAGER_H_

#include <list>
#include <mutex>
#include <utility>

namespace agora {
namespace iris {

template<class T>
class IrisObserverManager {
 private:
  struct ObserverEntry {
    ObserverEntry(T *observer, int order, std::string identifier)
        : observer_(observer), order_(order),
          identifier_(std::move(identifier)) {}

    T *observer_;
    int order_;
    std::string identifier_;
  };
  typedef std::list<ObserverEntry *> ObserverEntryList;

 public:
  void RegisterObserver(T *observer, int order, const char *identifier) {
    std::lock_guard<std::mutex> lock(mutex_);

    auto *observer_entry = new ObserverEntry(observer, order, identifier);
    if (observer_entry_list_.empty()) {
      observer_entry_list_.push_back(observer_entry);
      return;
    }

    // Insert before the first entry with a higher |order| value.
    auto it = observer_entry_list_.begin();
    for (; it != observer_entry_list_.end(); ++it) {
      if ((*it)->order_ > order) { break; }
    }

    observer_entry_list_.emplace(it, observer_entry);
  }

  void UnRegisterObserver(const char *identifier) {
    std::lock_guard<std::mutex> lock(mutex_);

    if (observer_entry_list_.empty()) return;

    auto it = observer_entry_list_.begin();
    while (it != observer_entry_list_.end()) {
      if ((*it)->identifier_ == identifier) {
        RemoveObserver(it);
      } else {
        ++it;
      }
    }
  }

  void RemoveObserver(typename ObserverEntryList::iterator &iterator) {
    ObserverEntry *current_entry = *(iterator);
    // Delete the provider entry now.
    iterator = observer_entry_list_.erase(iterator);
    delete current_entry;
  }

  void RemoveAllObservers() {
    std::lock_guard<std::mutex> lock(mutex_);

    if (observer_entry_list_.empty()) return;

    auto it = observer_entry_list_.begin();
    while (it != observer_entry_list_.end()) { RemoveObserver(it); }
  }

  unsigned int GetObserverCount() {
    std::lock_guard<std::mutex> lock(mutex_);

    return observer_entry_list_.size();
  }

  T *GetObserver(unsigned int index) {
    std::lock_guard<std::mutex> lock(mutex_);

    if (index >= observer_entry_list_.size()) { return nullptr; }
    auto it = std::next(observer_entry_list_.begin(), index);
    return (*it)->observer_;
  }

 private:
  ObserverEntryList observer_entry_list_;
  std::mutex mutex_;
};

}// namespace iris
}// namespace agora

#endif//IRIS_OBSERVER_MANAGER_H_
