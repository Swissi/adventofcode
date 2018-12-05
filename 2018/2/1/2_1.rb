doubles = 0
triples = 0
ids = File.readlines('i.txt')

ids.each do |id|
  a = id.split('')
  b = Hash.new(0)
#fzostwblnqkhpuzxirnevmaycq
  a.each do |v|
    b[v] += 1
  end

  doubles += 1 if b.value? 2
  triples += 1 if b.value? 3
end

puts doubles * triples